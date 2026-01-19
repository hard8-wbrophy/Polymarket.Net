using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;

namespace Polymarket.Net.Objects.Sockets
{
    internal class PolymarketQuery<T> : Query<T>
    {
        public PolymarketQuery(string type, string[] assets) : base(new PolymarketSocketRequest
        {
            Type = type,
            Assets = assets,
            CustomFeatureEnabled = true
        }, false, 1)
        {
            ExpectsResponse = false;

            MessageRouter = MessageRouter.CreateWithoutHandler<T>("");
        }
    }
}
