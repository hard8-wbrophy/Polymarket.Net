using Polymarket.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// User order update
    /// </summary>
    public record PolymarketOrderUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Owner
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;
        /// <summary>
        /// Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order owner
        /// </summary>
        [JsonPropertyName("order_owner")]
        public string OrderOwner { get; set; } = string.Empty;
        /// <summary>
        /// Original quantity
        /// </summary>
        [JsonPropertyName("original_size")]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("size_matched")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Order trades
        /// </summary>
        [JsonPropertyName("associate_trades")]
        public string[] Trades { get; set; } = [];
        /// <summary>
        /// Outcome
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;
        /// <summary>
        /// Order update type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderUpdateType UpdateType { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Expiration
        /// </summary>
        [JsonPropertyName("expiration")]
        public string Expiration { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Maker address
        /// </summary>
        [JsonPropertyName("maker_address")]
        public string MakerAddress { get; set; } = string.Empty;
    }
}
