using HybridNetworkLib.Common;
using HybridNetworkLib.Generic;
using UnityEngine;

namespace HybridNetworkLibUnity.Common
{
    public class NetworkConfiguration : ScriptableObject
    {
        [SerializeField] private TransportLayerInfo[] transportLayers;
        public TransportLayerInfo[] TransportLayers => transportLayers;

        [SerializeField] private IObjectSerializer serializer;
        public IObjectSerializer Serializer => serializer;
    }
}