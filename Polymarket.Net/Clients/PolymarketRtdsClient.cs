using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients
{
    /// <summary>
    /// A simple WebSocket client for the Polymarket Real-Time Data Socket (RTDS).
    /// Connects to <c>wss://ws-live-data.polymarket.com</c> and streams real-time
    /// Chainlink crypto price updates.
    /// </summary>
    public class PolymarketRtdsClient : IDisposable
    {
        private readonly string _endpoint;
        private readonly ILogger<PolymarketRtdsClient> _logger;
        private ClientWebSocket? _webSocket;
        private CancellationTokenSource? _cts;
        private Task? _receiveTask;
        private Task? _pingTask;
        private HashSet<string>? _subscribedSymbols;
        private bool _disposed;

        private static readonly TimeSpan PingInterval = TimeSpan.FromSeconds(30);
        private static readonly byte[] PingBytes = Encoding.UTF8.GetBytes("PING");
        private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(60);
        private const int MaxRetryAttempts = 10;

        /// <summary>
        /// Raised when a Chainlink price update is received
        /// </summary>
        public event Action<PolymarketRtdsMessage>? OnChainlinkPriceUpdate;

        /// <summary>
        /// Raised when the connection is established
        /// </summary>
        public event Action? OnConnected;

        /// <summary>
        /// Raised when the connection is closed
        /// </summary>
        public event Action? OnDisconnected;

        /// <summary>
        /// Raised when an error occurs
        /// </summary>
        public event Action<Exception>? OnError;

        /// <summary>
        /// Create a new instance of <see cref="PolymarketRtdsClient"/> using default addresses
        /// </summary>
        public PolymarketRtdsClient(ILogger<PolymarketRtdsClient>? logger = null)
            : this(PolymarketApiAddresses.Default.RtdsSocketClientAddress, logger)
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="PolymarketRtdsClient"/> with a custom endpoint
        /// </summary>
        /// <param name="endpoint">WebSocket endpoint URI</param>
        /// <param name="logger">Optional logger</param>
        public PolymarketRtdsClient(string endpoint, ILogger<PolymarketRtdsClient>? logger = null)
        {
            _endpoint = endpoint;
            _logger = logger ?? NullLogger<PolymarketRtdsClient>.Instance;
        }

        /// <summary>
        /// Connect and subscribe to Chainlink prices for the given symbols.
        /// Runs until the <paramref name="ct"/> is cancelled or the connection is closed.
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to (e.g., <c>"btc/usd"</c>, <c>"eth/usd"</c>)</param>
        /// <param name="ct">Cancellation token</param>
        public async Task ConnectAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var retryDelay = InitialRetryDelay;
            var attempt = 0;

            while (!ct.IsCancellationRequested)
            {
                attempt++;
                var token = _cts.Token;
                _webSocket = new ClientWebSocket();

                try
                {
                    _logger.LogInformation("Connecting to RTDS at {Endpoint} (attempt {Attempt})", _endpoint, attempt);
                    await _webSocket.ConnectAsync(new Uri(_endpoint), token).ConfigureAwait(false);
                    _logger.LogInformation("Connected to RTDS");
                    OnConnected?.Invoke();

                    await SendSubscriptionsAsync(symbols, token).ConfigureAwait(false);

                    _pingTask = RunPingLoopAsync(token);
                    _receiveTask = RunReceiveLoopAsync(token);

                    // Wait until one task exits, then stop the other so both shut down cleanly
                    await Task.WhenAny(_pingTask, _receiveTask).ConfigureAwait(false);
                    _cts.Cancel();
                    try { await Task.WhenAll(_pingTask, _receiveTask).ConfigureAwait(false); }
                    catch { /* tasks handle their own exceptions internally */ }

                    // Reset backoff on a successful session
                    retryDelay = InitialRetryDelay;
                    attempt = 0;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    _logger.LogInformation("RTDS connection cancelled");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RTDS connection error");
                    OnError?.Invoke(ex);
                }
                finally
                {
                    await CloseAsync().ConfigureAwait(false);
                    OnDisconnected?.Invoke();
                }

                if (ct.IsCancellationRequested)
                    break;

                if (attempt >= MaxRetryAttempts)
                {
                    _logger.LogError("RTDS max retry attempts ({MaxRetries}) reached, giving up", MaxRetryAttempts);
                    break;
                }

                _logger.LogInformation("Reconnecting in {Delay}s ...", retryDelay.TotalSeconds);
                try { await Task.Delay(retryDelay, ct).ConfigureAwait(false); }
                catch (OperationCanceledException) { break; }

                retryDelay = TimeSpan.FromSeconds(Math.Min(retryDelay.TotalSeconds * 2, MaxRetryDelay.TotalSeconds));

                // Create a fresh linked CTS for the next attempt
                _cts.Dispose();
                _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            }
        }

        /// <summary>
        /// Disconnect and close the WebSocket connection
        /// </summary>
        public async Task DisconnectAsync()
        {
            _cts?.Cancel();
            await CloseAsync().ConfigureAwait(false);
        }

        private async Task SendSubscriptionsAsync(IEnumerable<string> symbols, CancellationToken ct)
        {
            // The server maintains one subscription per topic and its filter
            // only accepts a single symbol string.  Subscribe to the whole
            // topic without a symbol filter so we receive updates for every
            // symbol, then filter client-side for the requested set.
            _subscribedSymbols = new HashSet<string>(symbols, StringComparer.OrdinalIgnoreCase);

            var message = new
            {
                action = "subscribe",
                subscriptions = new[]
                {
                    new
                    {
                        topic = "crypto_prices_chainlink",
                        type = "*"
                    }
                }
            };

            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket!.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct).ConfigureAwait(false);
            _logger.LogInformation("Sent subscription: {Message}", json);
        }

        private async Task RunPingLoopAsync(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested && _webSocket?.State == WebSocketState.Open)
                {
                    await Task.Delay(PingInterval, ct).ConfigureAwait(false);

                    if (_webSocket?.State == WebSocketState.Open)
                    {
                        await _webSocket.SendAsync(new ArraySegment<byte>(PingBytes), WebSocketMessageType.Text, true, ct).ConfigureAwait(false);
                        _logger.LogTrace("Sent PING");
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ping loop error");
            }
        }

        private async Task RunReceiveLoopAsync(CancellationToken ct)
        {
            var buffer = new byte[8192];

            try
            {
                while (!ct.IsCancellationRequested && _webSocket?.State == WebSocketState.Open)
                {
                    var messageBuilder = new StringBuilder();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct).ConfigureAwait(false);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _logger.LogInformation("RTDS server closed the connection");
                            return;
                        }

                        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    }
                    while (!result.EndOfMessage);

                    var raw = messageBuilder.ToString();
                    _logger.LogInformation("Received: {Message}", raw);

                    if (string.IsNullOrWhiteSpace(raw) || raw == "PONG")
                        continue;

                    if (raw.Contains("Too Many Requests"))
                    {
                        _logger.LogWarning("Rate-limited by server, will reconnect with backoff");
                        return;
                    }

                    TryHandleMessage(raw);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Receive loop error");
                OnError?.Invoke(ex);
            }
        }

        private void TryHandleMessage(string raw)
        {
            try
            {
                var message = JsonSerializer.Deserialize<PolymarketRtdsMessage>(raw);
                if (message == null)
                    return;

                if (message.Topic != "crypto_prices_chainlink")
                    return;

                // Client-side filter: only raise the event for symbols the
                // caller requested.  If no set was provided, pass everything.
                if (_subscribedSymbols != null
                    && message.Payload?.Symbol != null
                    && !_subscribedSymbols.Contains(message.Payload.Symbol))
                {
                    return;
                }

                OnChainlinkPriceUpdate?.Invoke(message);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to deserialize RTDS message: {Raw}", raw);
            }
        }

        private async Task CloseAsync()
        {
            if (_webSocket == null)
                return;

            try
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                        .ConfigureAwait(false);
                }
            }
            catch { }
            finally
            {
                _webSocket.Dispose();
                _webSocket = null;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _cts?.Cancel();
            _cts?.Dispose();
            _webSocket?.Dispose();
        }
    }
}
