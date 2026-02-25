using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// A message received from the Polymarket Real-Time Data Socket (RTDS)
    /// </summary>
    public record PolymarketRtdsMessage
    {
        /// <summary>
        /// The subscription topic (e.g., <c>crypto_prices_chainlink</c>)
        /// </summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// The message type/event (e.g., <c>update</c>)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Unix timestamp in milliseconds when the message was sent
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Chainlink price payload
        /// </summary>
        [JsonPropertyName("payload")]
        public PolymarketChainlinkPricePayload? Payload { get; set; }
    }

    /// <summary>
    /// Chainlink crypto price payload from the RTDS
    /// </summary>
    public record PolymarketChainlinkPricePayload
    {
        /// <summary>
        /// Trading pair symbol in slash-separated format (e.g., <c>btc/usd</c>, <c>eth/usd</c>)
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Unix timestamp in milliseconds when the price was recorded
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Current price value in the quote currency
        /// </summary>
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }
}
