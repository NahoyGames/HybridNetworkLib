using System;

namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class NetSerializableAttribute : System.Attribute
    {
        
    }
}