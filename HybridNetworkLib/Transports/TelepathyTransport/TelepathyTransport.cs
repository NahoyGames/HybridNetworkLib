using HybridNetworkLib.Common;
using HybridNetworkLib.Generic;
using HybridNetworkLib.Generic.Client;
using HybridNetworkLib.Generic.Server;

using Logger = HybridNetworkLib.Runtime.Logger;

namespace HybridNetworkLib.Transports.TelepathyTransport
{
    public class TelepathyTransport : ITransportLayer
    {
        #region COMMON

        public string Name => "Telepathy";
        private int _channelId;

        private void SetupLogger()
        {
            //Telepathy.Logger.Log = Logger.Log;
            Telepathy.Logger.Log = msg => Logger.Log("[Telepathy] " + msg);
            Telepathy.Logger.LogWarning = msg => Logger.Warn("[Telepathy] " + msg);
            Telepathy.Logger.LogError = msg => Logger.Error("[Telepathy] " + msg);
        }

        #endregion

        #region SERVER
        
        private Telepathy.Server _server;

        private OnClientConnect _serverConnectionCallback;
        private Generic.Server.OnReceiveMessage _serverMessageCallback;
        private OnClientDisconnect _serverDisconnectCallback;
        
        bool IServerTransportLayer.Init(int channelId, OnClientConnect connectionCallback, Generic.Server.OnReceiveMessage messageCallback, OnClientDisconnect disconnectCallback)
        {
            _server = new Telepathy.Server();
            _channelId = channelId;

            _serverConnectionCallback = connectionCallback;
            _serverMessageCallback = messageCallback;
            _serverDisconnectCallback = disconnectCallback;
            
            SetupLogger();
            
            return true;
        }

        bool IServerTransportLayer.Start(int port)
        {
            if (_server == null)
            {
                Logger.Warn("Server transport was never initialized and thus can't be started.");
                return false;
            }

            return _server.Start(port);
        }

        void IServerTransportLayer.Update()
        {
            while (_server.GetNextMessage(out var msg))
            {
                var id = new PerChannelID(_channelId, msg.connectionId);
                
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:
                        _serverConnectionCallback.Invoke(_server.GetClientAddress(msg.connectionId), id);
                        break;
                    case Telepathy.EventType.Data:
                        _serverMessageCallback.Invoke(msg.data, id);
                        break;
                    case Telepathy.EventType.Disconnected:
                        _serverDisconnectCallback.Invoke(id);
                        break;
                }
            }
        }
        
        bool IServerTransportLayer.Send(byte[] data, int id)
        {
            return _server.Send(id, data);
        }

        bool IServerTransportLayer.DisconnectClient(int id)
        {
            return _server.Disconnect(id);
        }

        bool IServerTransportLayer.Stop()
        {
            _server.Stop();
            return !_server.Active;
        }

        #endregion

        #region CLIENT

        private Telepathy.Client _client;

        private OnConnected _clientConnectedCallback;
        private Generic.Client.OnReceiveMessage _clientMessageCallback;
        private OnDisconnected _clientDisconnectedCallback;
        
        bool IClientTransportLayer.Init(int channelId, OnConnected connectedCallback, Generic.Client.OnReceiveMessage messageCallback, OnDisconnected disconnectedCallback)
        {
            _client = new Telepathy.Client();
            _channelId = channelId;

            _clientConnectedCallback = connectedCallback;
            _clientMessageCallback = messageCallback;
            _clientDisconnectedCallback = disconnectedCallback;
            
            SetupLogger();

            return true;
        }

        bool IClientTransportLayer.Connect(string address, int port)
        {
            if (_client == null)
            {
                Logger.Warn("Client transport was never initialized and thus can't connect.");
                return false;
            }
            
            _client.Connect(address, port);
            return _client.Connected || _client.Connecting;
        }

        void IClientTransportLayer.Update()
        {
            while (_client.GetNextMessage(out var msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:
                        _clientConnectedCallback.Invoke(_channelId);
                        break;
                    case Telepathy.EventType.Data:
                        _clientMessageCallback.Invoke(msg.data);
                        break;
                    case Telepathy.EventType.Disconnected:
                        _clientDisconnectedCallback.Invoke(_channelId);
                        break;
                }
            }
        }

        bool IClientTransportLayer.Send(byte[] data)
        {
            return _client.Send(data);
        }

        bool IClientTransportLayer.Disconnect()
        {
            _client.Disconnect();
            return !_client.Connected;
        }

        #endregion
    }
}