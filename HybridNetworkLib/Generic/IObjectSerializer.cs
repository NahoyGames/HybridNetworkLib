using System;

namespace HybridNetworkLib.Generic
{
    public interface IObjectSerializer
    {
        void RegisterObjectType(Type type);
        byte[] SerializeObject(object obj);
        object DeserializeObject(byte[] arr);
    }
}