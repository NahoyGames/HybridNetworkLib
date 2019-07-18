namespace HybridNetworkLib.Common
{
    public struct PerChannelID
    {
        public int Channel { get; }
        public int Id { get; }

        
        public PerChannelID(int channel, int id)
        {
            Channel = channel;
            Id = id;
        }
    }
}