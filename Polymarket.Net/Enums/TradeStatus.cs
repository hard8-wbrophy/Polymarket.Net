using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Trade status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeStatus>))]
    public enum TradeStatus
    {
        /// <summary>
        /// ["<c>MATCHED</c>"] Matched
        /// </summary>
        [Map("MATCHED")]
        Matched,
        /// <summary>
        /// ["<c>MINED</c>"] Mined
        /// </summary>
        [Map("MINED")]
        Mined,
        /// <summary>
        /// ["<c>CONFIRMED</c>"] Confirmed
        /// </summary>
        [Map("CONFIRMED")]
        Confirmed,
        /// <summary>
        /// ["<c>RETRYING</c>"] Retrying
        /// </summary>
        [Map("RETRYING")]
        Retrying,
        /// <summary>
        /// ["<c>FAILED</c>"] Failed
        /// </summary>
        [Map("FAILED")]
        Failed,
    }
}
