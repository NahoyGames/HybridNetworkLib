using System;
using System.Threading;
using CommonExample.Packets;
using HybridNetworkLib.Client;
using HybridNetworkLib.Common;
using HybridNetworkLib.Runtime;
using HybridNetworkLib.Serializers.CobblestoneSerializer;
using HybridNetworkLib.Transports.MiniUdpTransport;
using HybridNetworkLib.Transports.TelepathyTransport;

namespace ClientExample
{
    class Program
    {
        public static void Main(string[] args)
        {
            var client = new ClientNetManager
                (
                    new[]
                    {
                        new TransportLayerInfo(new TelepathyTransport(), 1337),
                        new TransportLayerInfo(new MiniUdpTransport(), 1447), 
                    },
                    new CobblestoneSerializer()
                );
            
            client.RegisterPacket(typeof(PacketMessage));
            client.Subscribe(packet =>
                {
                    if (packet is PacketMessage message)
                    {
                        Logger.Log("Server: " + message.text);
                    }
                });
            
            client.Connect("127.0.0.1");
            
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.A)
                    {
                        client.Send(new PacketMessage("Hello there, server!"), 0);
                    }
                    else if (key == ConsoleKey.B)
                    {
                        client.Send(new PacketMessage("Epic mate"), 1);
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        client.Send(new PacketMessage("I'm logging off..."), 0);
                        client.Disconnect();
                        break;
                    }
                }
                client.Update();
                Thread.Sleep(16);
            }
        }
    }
}