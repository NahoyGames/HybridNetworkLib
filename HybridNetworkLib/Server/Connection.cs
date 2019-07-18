using System.Collections.Generic;
using System.Net;
using HybridNetworkLib.Common;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Server
{
    public class Connection
    {
        public readonly IPAddress Address;
        
        private readonly Dictionary<int, int> _perChannelId;

        public Connection(IPAddress ip)
        {
            Address = ip;
            _perChannelId = new Dictionary<int, int>();
        }

        public bool AppendChannelId(PerChannelID id)
        {
            if (HasChannelId(id.Channel))
            {
                Logger.Warn("This connection is already connected on channel#" + id.Channel +".");
                return false;
            }
            
            _perChannelId.Add(id.Channel, id.Id);
            return true;
        }

        public bool HasChannelId(int channel)
        {
            return _perChannelId.ContainsKey(channel);
        }

        public int Id(int channel)
        {
            if (!HasChannelId(channel))
            {
                Logger.Error("Connection is not connected on this channel.");
                return -1;
            }

            return _perChannelId[channel];
        }
    }
}