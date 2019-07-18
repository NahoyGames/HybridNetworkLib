using HybridNetworkLib.Generic.Client;
using HybridNetworkLib.Generic.Server;

namespace HybridNetworkLib.Generic
{
    public interface ITransportLayer : IServerTransportLayer, IClientTransportLayer
    {
        /// <summary>
        /// The name of this transport layer. Used solely for debug purposes.
        /// </summary>
        string Name { get; }
    }
}