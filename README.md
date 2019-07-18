# HybridNetLib
One network-library to rule them all!

#### Warning
This project was started very recently, 13/07/19, and has not been extensively tested. It's very likely syntax changes will occur and bugs will appear. Please report errors/inconveniences and improve on this project via pull requests :-)
## Features
- Support for simultaneous UDP + TCP with their respective transport layer
	- Built-In: [Telepathy](http://github.com/vis2k/Telepathy "Telepathy"), a lightweight & efficient TCP transport layer
	- Built-In: [MiniUDP](http://github.com/ashoulson/MiniUDP "MiniUDP"), a barebones UDP transport layer(implemented as unreliable but fast)
	- Interface: [ITransportLayer](http://github.com/NahoyGames/HybridNetworkLib/blob/master/HybridNetworkLib/Generic/ITransportLayer.cs "ITransportLayer"), a simple and documented interface to (theoritcally) implement any transport layer into HybridNetLib
- Switching transport layers(from/to TCP, UDP, it doesn't matter) requires **1** line of code
- Support for any object → byte[] serializer
	- Built-In: CobblestoneSerializer, a very simple simple and expandible serializer which lets you sent *objects* over the network

## Sample
The following sample uses the built-in Telepathy & MiniUDP transport layers for networking, and built-in CobblestoneSerializer for serializing.
#### Client
```csharp
var client = new ClientNetManager(
	new TransportLayerInfo[] { new TransportLayerInfo(new TelepathyTransport(), 1337), new TransportLayerInfo(new MiniUdpTransport(), 1447) },
	new CobblestoneSerializer());
    
client.RegisterPacket(typeof(PacketMessage));
client.Subscribe(packet => {
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
			client.Send(new PacketMessage("Hello there, server!"), 0);
    	else if (key == ConsoleKey.B)
			client.Send(new PacketMessage("Epic mate"), 1);
    	else if (key == ConsoleKey.Q)
    	{
    		client.Disconnect();
    		break;
    	}
    }
    client.Update();
    Thread.Sleep(16);
}
```

#### Server
```csharp
var server = new ServerNetManager(
	new TransportLayerInfo[] { new TransportLayerInfo(new TelepathyTransport(), 1337), new TransportLayerInfo(new MiniUdpTransport(), 1447) },
	new CobblestoneSerializer());
    
server.RegisterPacket(typeof(PacketMessage));
server.Subscribe((packet, sender) => {
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
    		server.Send(new PacketMessage("Hey there, clients!"), 0);
    	else if (key == ConsoleKey.B)
    		server.Send(new PacketMessage("I'm gonna do what's called a pro-gamer move..."), 1);
    	else if (key == ConsoleKey.Q)
    	{
    		server.Stop();
    		break;
    	}
    }
    server.Update();
    Thread.Sleep(16);
}
```
#### Packet
```csharp
public struct PacketMessage
    {
    	/*
    	* The attribute [NetSerializable] must be assigned to any field/property which will be sent over the network.
    	*
    	* Supported: Public/Private/Protected access modifiers
    	* Supported: bool, byte, char, double, float, int, long, sbyte, short, string, uint, ulong, ushort, Arrays(any dimensions)
    	* Note: Arrays in the format T[,] won't work, but T[][] will. 
    	*/
    	[NetSerializable] public string text;
    
    	public PacketMessage(string text)
    	{
    		this.text = text;
    	}
    }
```
