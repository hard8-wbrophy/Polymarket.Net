namespace Polymarket.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class PolymarketApiAddresses
    {
        /// <summary>
        /// The address used by the PolymarketRestClient for the Clob API
        /// </summary>
        public string ClobRestClientAddress { get; set; } = "";

        /// <summary>
        /// The address used by the PolymarketRestClient for the Gamma API
        /// </summary>
        public string GammaRestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the PolymarketSocketClient for the Clob websocket API
        /// </summary>
        public string ClobSocketClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the PolymarketSocketClient for the Sport websocket API
        /// </summary>
        public string SportsSocketClientAddress { get; set; } = "";

        /// <summary>
        /// The address used by the PolymarketRtdsClient for the Real-Time Data Socket
        /// </summary>
        public string RtdsSocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Polymarket API
        /// </summary>
        public static PolymarketApiAddresses Default = new PolymarketApiAddresses
        {
            ClobRestClientAddress = "https://clob.polymarket.com",
            GammaRestClientAddress = "https://gamma-api.polymarket.com",
            ClobSocketClientAddress = "wss://ws-subscriptions-clob.polymarket.com",
            SportsSocketClientAddress = "wss://sports-api.polymarket.com",
            RtdsSocketClientAddress = "wss://ws-live-data.polymarket.com"
        };
    }
}
