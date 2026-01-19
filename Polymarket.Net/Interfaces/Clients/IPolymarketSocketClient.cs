using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Polymarket websocket API
    /// </summary>
    public interface IPolymarketSocketClient : ISocketClient
    {
        /// <summary>
        /// Clob API endpoints
        /// </summary>
        /// <see cref="IPolymarketSocketClientClobApi"/>
        public IPolymarketSocketClientClobApi ClobApi { get; }

        /// <summary>
        /// Update existing credentials which specify L1 credentials (PolymarketAddress, L1PrivateKey) with L2 credentials
        /// </summary>
        /// <param name="credentials">Credentials</param>
        void UpdateL2Credentials(PolymarketCreds credentials);

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
