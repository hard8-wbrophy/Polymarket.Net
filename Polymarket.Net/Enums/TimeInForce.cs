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
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TimeInForce>))]
    public enum TimeInForce
    {
        /// <summary>
        /// ["<c>FOK</c>"] Fill fully immediately or cancel
        /// </summary>
        [Map("FOK")]
        FillOrKill,
        /// <summary>
        /// ["<c>FAK</c>"] Fill at least partially immediately or cancel
        /// </summary>
        [Map("FAK")]
        ImmediateOrCancel,
        /// <summary>
        /// ["<c>GTC</c>"] Good until canceled
        /// </summary>
        [Map("GTC")]
        GoodTillCanceled,
        /// <summary>
        /// ["<c>GTD</c>"] Good till specific timestamp
        /// </summary>
        [Map("GTD")]
        GoodTillDate
    }
}
