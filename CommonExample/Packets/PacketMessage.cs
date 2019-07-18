using HybridNetworkLib.Serializers.CobblestoneSerializer;

namespace CommonExample.Packets
{
    public struct PacketMessage
    {
        [NetSerializable] public string text;

        public PacketMessage(string text)
        {
            this.text = text;
        }
    }
}