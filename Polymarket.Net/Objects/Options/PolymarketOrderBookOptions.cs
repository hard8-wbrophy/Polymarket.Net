using CryptoExchange.Net.Objects.Options;
using System;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Options for the Polymarket SymbolOrderBook
    /// </summary>
    public class PolymarketOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static PolymarketOrderBookOptions Default { get; set; } = new PolymarketOrderBookOptions();

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal PolymarketOrderBookOptions Copy()
        {
            var result = Copy<PolymarketOrderBookOptions>();
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
