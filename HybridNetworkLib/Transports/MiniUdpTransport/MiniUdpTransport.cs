using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using HybridNetworkLib.Common;
using HybridNetworkLib.Generic;
using HybridNetworkLib.Generic.Client;
using HybridNetworkLib.Generic.Server;
using OnReceiveMessage = HybridNetworkLib.Generic.Server.OnReceiveMessage;

namespace HybridNetworkLib.Transports.MiniUdpTransport
{
    public class MiniUdpTransport : ITransportLayer
    {
        #region COMMON

        string ITransportLayer.Name => "MiniUDP";
        private int _channelId;

        private const string ConnectionToken = "1";
        private  MiniUDP.NetCore _connection;
        
        #endregion

        #region SERVER

        private Dictionary<int, MiniUDP.NetPeer> _clients;
        private int _nextId;
        
        bool IServerTransportLayer.Init(int channelId, OnClientConnect connectCallback, OnReceiveMessage messageCallback, OnClientDisconnect disconnectCallback)
        {
            _connection = new MiniUDP.NetCore(ConnectionToken, true);
            _channelId = channelId;
            
            _clients = new Dictionary<int, MiniUDP.NetPeer>();
            _nextId = 0;

            _connection.PeerConnected += (peer, token) =>
                {
                    _clients.Add(_nextId, peer);
                    
                    var id = new PerChannelID(channelId, _nextId++);

                    connectCallback.Invoke(peer.EndPoint.Address, id);

                    peer.PayloadReceived += (netPeer, data, length) =>
                    {
                        if (data.Length != length) { Array.Resize(ref data, length); }

                        messageCallback.Invoke(data, id);
                    };
                };
            _connection.PeerClosed += (peer, reason, kickReason, error) =>
                {
                    var id = _clients.FirstOrDefault(i => i.Value == peer).Key;
                    disconnectCallback.Invoke(new PerChannelID(_channelId, id));
                    
                    _clients.Remove(id);
                };

            return true;
        }
        
        bool IServerTransportLayer.Start(int port)
        {
            _connection.Host(port);

            return true;
        }

        void IServerTransportLayer.Update()
        {
            _connection.PollEvents();
        }

        bool IServerTransportLayer.Send(byte[] data, int id)
        {
            return _clients[id]?.SendPayload(data, (ushort) data.Length) == SocketError.Success;
        }

        bool IServerTransportLayer.DisconnectClient(int id)
        {
            _clients[id].Close();

            return _clients[id].IsClosed;
        }

        bool IServerTransportLayer.Stop()
        {
            _connection.Stop();

            return true;
        }
        
        #endregion

        #region CLIENT

        private MiniUDP.NetPeer _host;

        bool IClientTransportLayer.Init(int channelId, OnConnected connectedCallback, Generic.Client.OnReceiveMessage messageCallback, OnDisconnected disconnectedCallback)
        {
            _connection = new MiniUDP.NetCore(ConnectionToken, false);
            _channelId = channelId;

            _connection.PeerConnected += (peer, token) =>
                {
                    // Peer will be the server
                    connectedCallback.Invoke(_channelId);

                    peer.PayloadReceived += (netPeer, data, length) =>
                    {
                        if (data.Length != length) { Array.Resize(ref data, length); }

                        messageCallback.Invoke(data);
                    };
                };
            _connection.PeerClosed += (peer, reason, kickReason, error) => disconnectedCallback.Invoke(_channelId);

            return true;
        }

        bool IClientTransportLayer.Connect(string address, int port)
        {
            _host = _connection.Connect(MiniUDP.NetUtil.StringToEndPoint(address + ":" + port), "");

            return true;
        }

        void IClientTransportLayer.Update()
        {
            _connection.PollEvents();
        }

        bool IClientTransportLayer.Send(byte[] data)
        {
            return _host.SendPayload(data, (ushort)data.Length) == SocketError.Success;
        }

        bool IClientTransportLayer.Disconnect()
        {
            _host.Close();
            return true;
        }
        
        #endregion
    }
}