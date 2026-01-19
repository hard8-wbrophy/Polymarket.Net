using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Order update type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderUpdateType>))]
    public enum OrderUpdateType
    {
        /// <summary>
        /// Order placed
        /// </summary>
        [Map("PLACEMENT")]
        Placed,
        /// <summary>
        /// Order updated
        /// </summary>
        [Map("UPDATE")]
        Update,
        /// <summary>
        /// Order canceled
        /// </summary>
        [Map("CANCELLATION")]
        Canceled,

    }
}
