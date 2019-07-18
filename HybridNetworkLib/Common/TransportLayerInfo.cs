using HybridNetworkLib.Generic;

namespace HybridNetworkLib.Common
{
    public struct TransportLayerInfo
    {
        public ITransportLayer Transport { get; }
        public int Port { get; }
        public string Name { get => Transport?.Name; }

        public TransportLayerInfo(ITransportLayer transportLayer, int port)
        {
            Transport = transportLayer;
            Port = port;
        }
    }
}