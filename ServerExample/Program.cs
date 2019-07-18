
using System;
using System.Threading;
using CommonExample.Packets;
using HybridNetworkLib.Common;
using HybridNetworkLib.Runtime;
using HybridNetworkLib.Serializers.CobblestoneSerializer;
using HybridNetworkLib.Server;
using HybridNetworkLib.Transports.MiniUdpTransport;
using HybridNetworkLib.Transports.TelepathyTransport;

namespace ServerExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * First we initialize our server by creating a HybridNetworkLib.Server.ServerNetManager
             *
             * This example passes in an array of transports, composed of the built-in Telepathy & MiniUDP
             * We're also using the built-in CobblestoneSerializer
             *
             * Important note: For any server and client to be connected, they must pass in the exact same transport layers & ports
             *
             * new ServerNetManager(ITransportLayer[], IObjectSerializer);
             */
            var server = new ServerNetManager
                (
                    new []
                    {
                        new TransportLayerInfo(new TelepathyTransport(), 1337),
                        new TransportLayerInfo(new MiniUdpTransport(), 1447), 
                    },
                    new CobblestoneSerializer()
                );
            
            /*
             * All packets must be registered with the serializer before being sent.
             *
             * It's important they're registered in the *same* order on both the server & client(s).
             */
            server.RegisterPacket(typeof(PacketMessage));
            
            /*
             * Subscribe this method to incoming packets
             */
            server.Subscribe((packet, sender) =>
                {
                    if (packet is PacketMessage message)
                    {
                        Logger.Log(sender.Address + ": " + message.text);
                    }
                });
            
            /*
             * Starts the server on the corresponding ports for each transport layer.
             */
            server.Start();

            /*
             * A simple forever-loop for this console application
             */
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.A)
                    {
                        /*
                         * Send a packet on channel#0, which in our case is Telepathy
                         */
                        server.Send(new PacketMessage("Hey there, clients!"), 0);
                    }
                    else if (key == ConsoleKey.B)
                    {
                        /*
                         * Send a packet on channel#1, which in our case is MiniUDP
                         */
                        server.Send(new PacketMessage("I'm gonna do what's called a pro-gamer move..."), 1);
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        /*
                         * Stop the server before quitting
                         */
                        server.Stop();
                        break;
                    }
                }
                /*
                 * Update checks for incoming connections & packets.
                 */
                server.Update();
                
                Thread.Sleep(16); // Simple ~60fps loop. If using Unity, make sure the code above is in FixedUpdate()
            }
        }
    }
}