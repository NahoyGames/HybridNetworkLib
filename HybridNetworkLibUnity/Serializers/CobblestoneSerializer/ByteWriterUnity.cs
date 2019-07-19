using HybridNetworkLib.Serializers.CobblestoneSerializer;
using UnityEngine;

namespace HybridNetworkLibUnity.Serializers.CobblestoneSerializer
{
    public class ByteWriterUnity : ByteWriter
    {
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Vector2 value) => SizeOf(value.x) + SizeOf(value.y);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Vector3 value) => SizeOf(value.x) + SizeOf(value.y) + SizeOf(value.z);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Vector4 value) => SizeOf(value.x) + SizeOf(value.y) + SizeOf(value.z) + SizeOf(value.w);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Vector2Int value) => SizeOf(value.x) + SizeOf(value.y);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Vector3Int value) => SizeOf(value.x) + SizeOf(value.y) + SizeOf(value.z);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(Quaternion value) => SizeOf(value.x) + SizeOf(value.y) + SizeOf(value.z) + SizeOf(value.w);
        
        /// <summary>
        /// Will attempt to return the size of the given value, if known.
        /// </summary>
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public override int TrySizeOf(object value)
        {
            switch (value)
            {
                case Vector2 a:
                    return SizeOf(a);
                case Vector3 b:
                    return SizeOf(b);
                case Vector4 c:
                    return SizeOf(c);
                case Vector2Int d:
                    return SizeOf(d);
                case Vector3Int e:
                    return SizeOf(e);
                case Quaternion f:
                    return SizeOf(f);
                default:
                    return base.TrySizeOf(value);
            }
        }
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Vector2 value) => Write(value.x) && Write(value.y);
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Vector3 value) => Write(value.x) && Write(value.y) && Write(value.z);
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Vector4 value) => Write(value.x) && Write(value.y) && Write(value.z) && Write(value.w);
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Vector2Int value) => Write(value.x) && Write(value.y);
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Vector3Int value) => Write(value.x) && Write(value.y) && Write(value.z);
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(Quaternion value) => Write(value.x) && Write(value.y) && Write(value.z) && Write(value.w);

        /// <summary>
        /// Will attempt to write the given value to the buffer, if known
        /// </summary>
        public override bool TryWrite(object value)
        {
            switch (value)
            {
                case Vector2 a:
                    return Write(a);
                case Vector3 b:
                    return Write(b);
                case Vector4 c:
                    return Write(c);
                case Vector2Int d:
                    return Write(d);
                case Vector3Int e:
                    return Write(e);
                case Quaternion f:
                    return Write(f);
                default:
                    return base.TryWrite(value);
            }
        }
    }
}