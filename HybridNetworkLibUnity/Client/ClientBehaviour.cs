using System;
using HybridNetworkLib.Client;
using UnityEngine;
using Logger = HybridNetworkLib.Runtime.Logger;

namespace HybridNetworkLibUnity.Client
{
    public class ClientBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            if (ClientNetManagerUnity.Instance == null)
            {
                Logger.Error("Cannot awaken a ClientBehaviour before the ClientNetManager! Try changing your script execution order.");
            }
            ClientNetManagerUnity.Instance.ListenForPackets(OnReceivePacket);
        }

        protected virtual void OnReceivePacket(object packet)
        {
            
        }

        protected void Send(int channel, object packet)
        {
            ClientNetManagerUnity.Instance.Send(channel, packet);
        }
    }
}