using System;
using System.Collections.Generic;
using System.Net;
using HybridNetworkLib.Common;
using HybridNetworkLib.Generic;
using HybridNetworkLib.Generic.Server;
using HybridNetworkLib.Runtime;
using OnReceiveMessage = HybridNetworkLib.Generic.Server.OnReceiveMessage;

namespace HybridNetworkLib.Server
{
    public class ServerNetManager
    {
        // Transport layers this server will be using
        private readonly TransportLayerInfo[] _transportLayers;

        // Serializer responsible for converting objects to bytes and back
        private readonly IObjectSerializer _serializer;
        
        // List of all clients currently connected to the server on all channels
        private readonly List<Connection> _activeConnections;
        // List of all clients ever connected to the server on all channels since start
        private readonly List<Connection> _connectionsHistory;
        // List of clients currently connecting on each channel procedurally
        private readonly Dictionary<IPAddress, Connection> _pendingConnections;

        // Callbacks from transport layers
        private event OnClientConnect ConnectCallback;
        private event OnReceiveMessage MessageCallback;
        private event OnClientDisconnect DisconnectCallback;
        
        // Subscribers to message events
        public delegate void OnReceivePacket(object packet, Connection sender);
        private event OnReceivePacket PacketSubscribers;

        public ServerNetManager(TransportLayerInfo[] transportLayers, IObjectSerializer serializer)
        {
            _transportLayers = transportLayers;
            _serializer = serializer;
            
            _activeConnections = new List<Connection>();
            _connectionsHistory = new List<Connection>();
            _pendingConnections = new Dictionary<IPAddress, Connection>();

            ConnectCallback = ClientConnectHandler;
            MessageCallback = ReceiveMessageHandler;
            DisconnectCallback = ClientDisconnectHandler;
            
            Init();
        }

        private bool Init()
        {
            for (int i = 0; i < _transportLayers.Length; i++)
            {
                if (!((IServerTransportLayer) _transportLayers[i].Transport).Init(i, ConnectCallback, MessageCallback, DisconnectCallback))
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

        public bool Start()
        {
            foreach (var layer in _transportLayers)
            {
                if (!((IServerTransportLayer) layer.Transport).Start(layer.Port))
                {
                    Logger.Error("Could not start the transport \"" + layer.Name + "\" on port " + layer.Port);
                    return false;
                }
            }

            return true;
        }

        public void Update()
        {
            foreach (var layer in _transportLayers)
            {
                ((IServerTransportLayer) layer.Transport).Update();
            }
        }

        private void Send(Connection receiver, byte[] serializedPacket, int channel)
        {
            ((IServerTransportLayer)_transportLayers[channel].Transport).Send(serializedPacket, receiver.Id(channel));
        }

        public void Send(Connection receiver, object packet, int channel)
        {
            Send(receiver, _serializer.SerializeObject(packet), channel);
        }

        public void Send(object packet, int channel)
        {
            var serializedPacket = _serializer.SerializeObject(packet);
            foreach (var connection in _activeConnections)
            {
                Send(connection, serializedPacket, channel);
            }
        }

        public bool Disconnect(Connection connection)
        {
            bool success = true;
            for (int i = 0; i < _transportLayers.Length; i++)
            {
                success &= ((IServerTransportLayer) _transportLayers[i].Transport).DisconnectClient(connection.Id(i));
            }

            return success;
        }

        public bool Stop()
        {
            bool success = true;
            foreach (var layer in _transportLayers)
            {
                success &= layer.Transport.Stop();
            }

            return success;
        }

        public Connection GetConnection(PerChannelID id, bool onlyConnected = true)
        {
            foreach (var connection in (onlyConnected ? _activeConnections : _connectionsHistory))
            {
                if (connection.Id(id.Channel) == id.Id)
                {
                    return connection;
                }
            }

            return null;
        }

        public Connection[] GetConnections(bool onlyConnected = true)
        {
            return onlyConnected ? _activeConnections.ToArray() : _connectionsHistory.ToArray();
        }
        
        private void ClientConnectHandler(IPAddress ip, PerChannelID id)
        {
            ip = ip.MapToIPv6();
            
            if (!_pendingConnections.ContainsKey(ip)) // Start pending
            {
                Logger.Log("Starting pending connection on \"" + ip + "\"...");
                _pendingConnections.Add(ip, new Connection(ip));
            }
            var connection = _pendingConnections[ip];

            connection.AppendChannelId(id); // Append channel

            var complete = true; // Pending connection is connected on all channels?
            for (int i = 0; i < _transportLayers.Length; i++)
            {
                complete &= connection.HasChannelId(i);
            }
            
            if (complete) // Successful connection on all channels
            {
                _pendingConnections.Remove(ip);
                _connectionsHistory.Add(connection);
                _activeConnections.Add(connection);
                
                Logger.Log("A client successfully connected on all channels.");
            }
        }

        private void ReceiveMessageHandler(byte[] data, PerChannelID id)
        {
            Logger.Log("Received packet!");
            PacketSubscribers?.Invoke(_serializer.DeserializeObject(data), GetConnection(id, false));
        }

        private void ClientDisconnectHandler(PerChannelID id)
        {
            // Disconnect client on all channels
            Disconnect(GetConnection(id, false));
        }
    }
}