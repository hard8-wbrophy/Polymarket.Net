using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Sport update
    /// </summary>
    public record PolymarketSportsUpdate
    {
        /// <summary>
        /// Game id
        /// </summary>
        [JsonPropertyName("gameId")]
        public long GameId { get; set; }
        /// <summary>
        /// League abbreviation
        /// </summary>
        [JsonPropertyName("leagueAbbreviation")]
        public string LeagueAbbreviation { get; set; } = string.Empty;
        /// <summary>
        /// Home team name
        /// </summary>
        [JsonPropertyName("homeTeam")]
        public string HomeTeam { get; set; } = string.Empty;
        /// <summary>
        /// Away team name
        /// </summary>
        [JsonPropertyName("awayTeam")]
        public string AwayTeam { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Score
        /// </summary>
        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;
        /// <summary>
        /// Period
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;
        /// <summary>
        /// Elapsed
        /// </summary>
        [JsonPropertyName("elapsed")]
        public string? Elapsed { get; set; }
        /// <summary>
        /// Turn
        /// </summary>
        [JsonPropertyName("turn")]
        public string? Turn { get; set; }
        /// <summary>
        /// Is live
        /// </summary>
        [JsonPropertyName("live")]
        public bool Live { get; set; }
        /// <summary>
        /// Has ended
        /// </summary>
        [JsonPropertyName("ended")]
        public bool Ended { get; set; }
    }
}
