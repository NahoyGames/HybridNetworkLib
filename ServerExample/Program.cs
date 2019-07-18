
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
            var server = new ServerNetManager
                (
                    new []
                    {
                        new TransportLayerInfo(new TelepathyTransport(), 1337),
                        new TransportLayerInfo(new MiniUdpTransport(), 1447), 
                    },
                    new CobblestoneSerializer()
                );
            
            server.RegisterPacket(typeof(PacketMessage));
            server.Subscribe((packet, sender) =>
                {
                    if (packet is PacketMessage message)
                    {
                        Logger.Log(sender.Address + ": " + message.text);
                    }
                });
            
            server.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.A)
                    {
                        server.Send(new PacketMessage("Hey there, clients!"), 0);
                    }
                    else if (key == ConsoleKey.B)
                    {
                        server.Send(new PacketMessage("I'm gonna do what's called a pro-gamer move..."), 1);
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        server.Stop();
                        break;
                    }
                }
                server.Update();
                Thread.Sleep(16);
            }
        }
    }
}