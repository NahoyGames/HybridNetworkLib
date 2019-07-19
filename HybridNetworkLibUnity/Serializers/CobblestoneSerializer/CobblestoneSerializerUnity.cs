using HybridNetworkLib.Serializers.CobblestoneSerializer;

namespace HybridNetworkLibUnity.Serializers.CobblestoneSerializer
{
    public class CobblestoneSerializerUnity : HybridNetworkLib.Serializers.CobblestoneSerializer.CobblestoneSerializer
    {
        public override ByteWriter GetWriter() => new ByteWriterUnity();
        public override ByteReader GetReader(byte[] data) => new ByteReader(data);
    }
}