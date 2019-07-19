namespace HybridNetworkLibUnity.Common
{
    public struct NetworkInstanceId
    {
        /// <summary>
        /// The identification number of this entity.
        /// 
        /// There are 2 packets for spawning entities, one which uses a byte for identity and another ushort.
        /// Therefore, the server will stop spawning after (2^8 + 2^16) entities total.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The unique identification number of this entity.
        ///
        /// No other entity may have the same unique-id.
        /// </summary>
        public ulong UniqueId { get; private set; }

        public NetworkInstanceId(int id, ulong uniqueId)
        {
            Id = id;
            UniqueId = uniqueId;
        }
    }
}