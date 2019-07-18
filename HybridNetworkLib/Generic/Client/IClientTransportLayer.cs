namespace HybridNetworkLib.Generic.Client
{
    /// <summary>
    /// Delegate called whenever the client is connected to the server on this channel
    /// </summary>
    public delegate void OnConnected(int channel);

    /// <summary>
    /// Delegate called whenever a client sends data on this channel.
    /// </summary>
    /// <param name="data">Raw data received</param>
    public delegate void OnReceiveMessage(byte[] data);

    /// <summary>
    /// Delegate called whenever the client is disconnected from the server on this channel
    /// </summary>
    public delegate void OnDisconnected(int channel);
    
    public interface IClientTransportLayer
    {
        /// <summary>
        /// Initialize the client transport.
        /// </summary>
        /// <param name="channelId">This transport channel ID. Important to keep track of somewhere, will be used later on.</param>
        /// <param name="connectedCallback">Callback which MUST be called when the client is connected on this channel.</param>
        /// <param name="messageCallback">Callback which MUST be called when a message is received from the server.</param>
        /// <param name="disconnectedCallback">Callback which mUST be called when the client is disconnected on this channel.</param>
        /// <returns>Success of the initialization.</returns>
        bool Init(int channelId, OnConnected connectedCallback, OnReceiveMessage messageCallback, OnDisconnected disconnectedCallback);
        
        /// <summary>
        /// Connects the client to the server at the given address on the given port.
        /// </summary>
        /// <param name="address">IP address of the server.</param>
        /// <param name="port">Port for this channel.</param>
        /// <returns>Success of the start operation.</returns>
        bool Connect(string address, int port);

        /// <summary>
        /// Listen for incoming messages and connection updates, notifying the corresponding callback when either is received. Most often will be called every frame.
        /// </summary>
        void Update();

        /// <summary>
        /// Send raw data to the server.
        /// </summary>
        /// <param name="data">The data to be sent on this channel</param>
        /// <returns>Success of the operation. Can yield false for reasons such as a disconnection or packet loss on transports like RUDP.</returns>
        bool Send(byte[] data);

        /// <summary>
        /// Disconnect the client from the server on this channel.
        /// </summary>
        /// <returns></returns>
        bool Disconnect();
    }
}