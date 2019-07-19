using HybridNetworkLib.Server;
using HybridNetworkLibUnity.Common;
using UnityEngine;

namespace HybridNetworkLibUnity.Server.Components
{
    public class ServerNetworkIdentity : MonoBehaviour
    {
        public NetworkInstanceId Id { get; private set; }

        /// <summary>
        /// The [player] owner of this object, if any.
        /// </summary>
        public Connection Owner { get; private set; }
    }
}