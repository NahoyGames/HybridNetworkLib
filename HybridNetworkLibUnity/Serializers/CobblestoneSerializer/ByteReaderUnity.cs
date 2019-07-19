using System;
using HybridNetworkLib.Serializers.CobblestoneSerializer;
using UnityEngine;

namespace HybridNetworkLibUnity.Serializers.CobblestoneSerializer
{
    public class ByteReaderUnity : ByteReader
    {
        public ByteReaderUnity(byte[] data) : base(data)
        {
            
        }

        /// <returns>The value from the serialized data.</returns>
        public Vector2 ReadVector2(bool peek = false)
        {
            return new Vector2(ReadFloat(peek), ReadFloat(peek));
        }
        
        /// <returns>The value from the serialized data.</returns>
        public Vector3 ReadVector3(bool peek = false)
        {
            return new Vector3(ReadFloat(peek), ReadFloat(peek), ReadFloat(peek));
        }
        
        /// <returns>The value from the serialized data.</returns>
        public Vector4 ReadVector4(bool peek = false)
        {
            return new Vector4(ReadFloat(peek), ReadFloat(peek), ReadFloat(peek), ReadFloat(peek));
        }
        
        /// <returns>The value from the serialized data.</returns>
        public Vector2Int ReadVector2Int(bool peek = false)
        {
            return new Vector2Int(ReadInt(peek), ReadInt(peek));
        }
        
        /// <returns>The value from the serialized data.</returns>
        public Vector3Int ReadVector3Int(bool peek = false)
        {
            return new Vector3Int(ReadInt(peek), ReadInt(peek), ReadInt(peek));
        }
        
        /// <returns>The value from the serialized data.</returns>
        public Quaternion ReadQuaternion(bool peek = false)
        {
            return new Quaternion(ReadFloat(peek), ReadFloat(peek), ReadFloat(peek), ReadFloat(peek));
        }

        /// <summary>
        /// Will attempt to call the correct Read[...]() method if type is known
        /// </summary>
        public override object TryRead(Type type, bool peek = false)
        {
            switch (true)
            {
                case bool _ when type == typeof(Vector2): return ReadVector2();
                case bool _ when type == typeof(Vector3): return ReadVector3();
                case bool _ when type == typeof(Vector4): return ReadVector4();
                case bool _ when type == typeof(Vector2Int): return ReadVector2Int();
                case bool _ when type == typeof(Vector3Int): return ReadVector3Int();
                case bool _ when type == typeof(Quaternion): return ReadQuaternion();
                default: return base.TryRead(type, peek);
            }
        }
    }
}