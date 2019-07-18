using System.Net;
using HybridNetworkLib.Common;

namespace HybridNetworkLib.Generic.Server
{
    /// <summary>
    /// Delegate called whenever a client connects on this channel.
    /// </summary>
    /// <param name="ip">The client's IP address.</param>
    /// <param name="id">The ID of the client on this channel.</param>
    public delegate void OnClientConnect(IPAddress ip, PerChannelID id);

    /// <summary>
    /// Delegate called whenever a client sends data on this channel.
    /// </summary>
    /// <param name="data">Raw data received.</param>
    /// <param name="id">The ID of the client on this channel.</param>
    public delegate void OnReceiveMessage(byte[] data, PerChannelID id);

    /// <summary>
    /// Delegate called whenever a client disconnects on this channel.
    /// </summary>
    /// <param name="id">The ID of the client on this channel.</param>
    public delegate void OnClientDisconnect(PerChannelID id);
    
    public interface IServerTransportLayer
    {
        /// <summary>
        /// Initialize the server transport.
        /// </summary>
        /// <param name="channelId">This transport channel ID. Important to keep track of somewhere, will be used later on.</param>
        /// <param name="connectCallback">Callback which MUST be called when a client connects on this channel.</param>
        /// <param name="messageCallback">Callback which MUST be called when a message is received from a client.</param>
        /// <param name="disconnectCallback">Callback which MUST be called when a client disconnects on this channel.</param>
        /// <returns>Success of the initialization.</returns>
        bool Init(int channelId, OnClientConnect connectCallback, OnReceiveMessage messageCallback, OnClientDisconnect disconnectCallback);

        /// <summary>
        /// Starts the server on the given port.
        /// </summary>
        /// <param name="port">Port for this channel.</param>
        /// <returns>Success of the start operation.</returns>
        bool Start(int port);

        /// <summary>
        /// Listen for incoming connections and messages, notifying the corresponding callback when either is received. Most often will be called every frame.
        /// </summary>
        void Update();

        /// <summary>
        /// Sends raw data to a given client.
        /// </summary>
        /// <param name="data">The data to be sent on this channel.</param>
        /// <param name="id">The ID of the receiving client.</param>
        /// <returns>Success of the operation. Can yield false for reasons such as a disconnection or packet loss on transports like RUDP</returns>
        bool Send(byte[] data, int id);
        
        /// <summary>
        /// Force disconnect a client from the server on this channel.
        /// </summary>
        /// <param name="id">The client's ID on this channel.</param>
        /// <returns>Whether kick was successful or not.</returns>
        bool DisconnectClient(int id);

        /// <summary>
        /// Stops the server.
        /// </summary>
        /// <returns>Whether the server was successfully terminated.</returns>
        bool Stop();
    }
}