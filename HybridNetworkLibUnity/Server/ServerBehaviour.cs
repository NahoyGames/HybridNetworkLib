using System;
using HybridNetworkLib.Server;
using UnityEngine;
using Logger = HybridNetworkLib.Runtime.Logger;

namespace HybridNetworkLibUnity.Server
{
    public class ServerBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            if (ServerNetManagerUnity.Instance == null)
            {
                Logger.Error("Cannot awaken a ServerBehaviour before the ServerNetManager! Try changing your script execution order.");
            }
            ServerNetManagerUnity.Instance.ListenForPackets(OnReceivePacket);
        }

        protected virtual void OnReceivePacket(object packet, Connection sender)
        {
            
        }

        protected void Send(Connection receiver, int channel, object packet)
        {
            ServerNetManagerUnity.Instance.Send(receiver, channel, packet);
        }
    }
}