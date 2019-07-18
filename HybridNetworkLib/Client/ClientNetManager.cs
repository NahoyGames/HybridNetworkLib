using System;
using HybridNetworkLib.Common;
using HybridNetworkLib.Generic;
using HybridNetworkLib.Generic.Client;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Client
{
    public class ClientNetManager
    {
        // Transport layers this client will be using
        private readonly TransportLayerInfo[] _transportLayers;
        
        // Serializer responsible for converting objects to bytes and back
        private readonly IObjectSerializer _serializer;

        private bool[] _pendingChannels;
        
        // Callbacks from transport layers
        private event OnConnected ConnectedCallback;
        private event OnReceiveMessage MessageCallback;
        private event OnDisconnected DisconnectedCallback;
        
        // Subscribers to message events
        public delegate void OnReceivePacket(object packet);

        private event OnReceivePacket PacketSubscribers;
        
        public ClientNetManager(TransportLayerInfo[] transportLayers, IObjectSerializer serializer)
        {
            _transportLayers = transportLayers;
            _serializer = serializer;
            
            ConnectedCallback = ConnectedHandler;
            MessageCallback = ReceiveMessageHandler;
            DisconnectedCallback = DisconnectedHandler;

            Init();
        }

        private bool Init()
        {
            for (int i = 0; i < _transportLayers.Length; i++)
            {
                if (!((IClientTransportLayer) _transportLayers[i].Transport).Init(i, ConnectedCallback, MessageCallback, DisconnectedCallback))
                {
                    Logger.Error("Could not initialize the transport \"" + _transportLayers[i].Name + "\"");
                    return false;
                }
            }

            return true;
        }
        
        public void RegisterPacket(Type type)
        {
            _serializer.RegisterObjectType(type);
        }

        public void Subscribe(OnReceivePacket handler)
        {
            PacketSubscribers += handler;
        }
        
        public bool Connect(string ip)
        {
            _pendingChannels = new bool[_transportLayers.Length];

            Logger.Log("Connecting...");
            
            foreach (var layer in _transportLayers)
            {
                if (!((IClientTransportLayer) layer.Transport).Connect(ip, layer.Port))
                {
                    Logger.Error("Could not connect the transport \"" + layer.Name + "\" to " + ip + " on port " + layer.Port);
                    return false;
                }
            }

            return true;
        }

        public void Update()
        {
            foreach (var layer in _transportLayers)
            {
                ((IClientTransportLayer) layer.Transport).Update();
            }
        }

        public void Send(object packet, int channel)
        {
            ((IClientTransportLayer) _transportLayers[channel].Transport).Send(_serializer.SerializeObject(packet));
        }

        public bool Disconnect()
        {
            bool success = true;
            foreach (var layer in _transportLayers)
            {
                success &= ((IClientTransportLayer) layer.Transport).Disconnect();
            }

            return success;
        }

        private void ConnectedHandler(int channel)
        {
            Logger.Log("Connected on channel#" + channel);
            _pendingChannels[channel] = true;

            var complete = true;
            foreach (var c in _pendingChannels)
            {
                complete &= c;
            }

            if (complete)
            {
                _pendingChannels = null;
                
                Logger.Log("Successfully connected on all channels!");
            }
        }

        private void ReceiveMessageHandler(byte[] data)
        {
            Logger.Log("Received packet!");
            PacketSubscribers?.Invoke(_serializer.DeserializeObject(data));
        }

        private void DisconnectedHandler(int channel)
        {
            Logger.Log("Disconnected...");
            Disconnect();
        }
    }
}