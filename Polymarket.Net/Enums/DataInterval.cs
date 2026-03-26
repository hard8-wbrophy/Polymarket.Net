using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Data interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DataInterval>))]
    public enum DataInterval
    {
        /// <summary>
        /// ["<c>1h</c>"] One hour
        /// </summary>
        [Map("1h")]
        OneHour,
        /// <summary>
        /// ["<c>6h</c>"] Six hours
        /// </summary>
        [Map("6h")]
        SixHours,
        /// <summary>
        /// ["<c>1d</c>"] One day
        /// </summary>
        [Map("1d")]
        OneDay,
        /// <summary>
        /// ["<c>1w</c>"] One week
        /// </summary>
        [Map("1w")]
        OneWeek,
        /// <summary>
        /// ["<c>1m</c>"] One month
        /// </summary>
        [Map("1m")]
        OneMonth,
        /// <summary>
        /// ["<c>max</c>"] Max
        /// </summary>
        [Map("max")]
        Max
    }
}
