using System;

using HybridNetworkLib.Client;
using HybridNetworkLibUnity.Common;
using Logger = HybridNetworkLib.Runtime.Logger;

using UnityEngine;

namespace HybridNetworkLibUnity.Client
{
    public class ClientNetManagerUnity : MonoBehaviour
    {
        [SerializeField] private NetworkConfiguration config;
        [SerializeField] private string serverAddress = "127.0.0.1";
        
        [SerializeField] private bool startOnAwake;

        private ClientNetManager _client;
        
        public static ClientNetManagerUnity Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            Logger.SetLogMethod(Debug.Log);
            Logger.SetWarnMethod(Debug.LogWarning);
            Logger.SetErrorMethod(Debug.LogError);

            if (startOnAwake)
            {
                ConnectClient(serverAddress);
            }
        }

        private void FixedUpdate()
        {
            _client.Update();
        }

        public bool ConnectClient(string ip)
        {
            if (_client == null)
            {
                _client = new ClientNetManager(config.TransportLayers, config.Serializer);
            }

            return _client.Connect(ip);
        }

        public bool DisconnectClient()
        {
            if (_client == null)
            {
                Logger.Error("Cannot disconnect the client before initialization!");
                return false;
            }

            return _client.Disconnect();
        }

        public void ListenForPackets(ClientNetManager.OnReceivePacket handler)
        {
            _client.Subscribe(handler);
        }

        public void RegisterPacket(Type type)
        {
            _client.RegisterPacket(type);
        }

        public void Send(int channel, object packet)
        {
            _client.Send(packet, channel);
        }
    }
}