using System;

using HybridNetworkLib.Common;
using HybridNetworkLib.Server;
using HybridNetworkLibUnity.Common;
using Logger = HybridNetworkLib.Runtime.Logger;

using UnityEngine;

namespace HybridNetworkLibUnity.Server
{
    public class ServerNetManagerUnity : MonoBehaviour
    {
        [SerializeField] private NetworkConfiguration config;

        [SerializeField] private bool startOnAwake;
        
        private ServerNetManager _server;
        
        public static ServerNetManagerUnity Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            Logger.SetLogMethod(Debug.Log);
            Logger.SetWarnMethod(Debug.LogWarning);
            Logger.SetErrorMethod(Debug.LogError);

            if (startOnAwake)
            {
                StartServer();
            }
        }

        private void FixedUpdate()
        {
            _server.Update();
        }

        /// <summary>
        /// Initialize & start the server with the provided transports & serializer
        /// </summary>
        public bool StartServer()
        {
            if (_server == null)
            {
                _server = new ServerNetManager(config.TransportLayers, config.Serializer);
            }
            
            return _server.Start();
        }

        /// <summary>
        /// Stop the server & disconnect all clients
        /// </summary>
        public bool StopServer()
        {
            if (_server == null)
            {
                Logger.Error("Cannot stop the server as it was never started!");
                return false;
            }
            return _server.Stop();
        }

        /// <summary>
        /// Kick an unwanted client on all channels
        /// </summary>
        public bool KickClient(Connection connection)
        {
            return _server.Disconnect(connection);
        }

        /// <summary>
        /// Find a client based off its channel id
        /// </summary>
        public Connection FindClient(PerChannelID id, bool onlyConnected = true)
        {
            return _server.GetConnection(id, onlyConnected);
        }

        /// <summary>
        /// Find a client based off its channel id
        /// </summary>
        public Connection FindClient(int channel, int id, bool onlyConnected = true)
        {
            return FindClient(new PerChannelID(channel, id), onlyConnected);
        }

        /// <summary>
        /// Subscribe a method to incoming packets
        /// </summary>
        public void ListenForPackets(ServerNetManager.OnReceivePacket handler)
        {
            _server.Subscribe(handler);
        }

        /// <summary>
        /// Register a typeof struct or class to be sent over the network
        /// </summary>
        public void RegisterPacket(Type type)
        {
            _server.RegisterPacket(type);
        }

        /// <summary>
        /// Send a packet to the given client on a given channel
        /// </summary>
        public void Send(Connection receiver, int channel, object packet)
        {
            _server.Send(receiver, packet, channel);
        }

        /// <summary>
        /// Send packet on the channel to the given id
        /// </summary>
        public void Send(PerChannelID receiver, object packet)
        {
            _server.Send(FindClient(receiver, true), packet, receiver.Channel);
        }

        /// <summary>
        /// Send packet on the channel to every active clients
        /// </summary>
        public void Send(int channel, object packet)
        {
            _server.Send(packet, channel);
        }
    }
}