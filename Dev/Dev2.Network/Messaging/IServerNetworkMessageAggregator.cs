﻿using System;
using Dev2.Network.Messages;

namespace Dev2.Network
{
    public interface IServerNetworkMessageAggregator<ContextT> where ContextT : NetworkContext, new()
    {
        void Publish(INetworkMessage message, bool async = true);
        void Publish(INetworkMessage message, IServerNetworkChannelContext<ContextT> context, bool async = true);
        Guid Subscribe<T>(Action<T, IServerNetworkChannelContext<ContextT>> callback) where T : INetworkMessage, new();
        Guid Subscribe<T>(Action<T, NetworkContext> callback) where T : INetworkMessage;
        bool Unsubscibe(Guid subscriptionToken);
    }
}
