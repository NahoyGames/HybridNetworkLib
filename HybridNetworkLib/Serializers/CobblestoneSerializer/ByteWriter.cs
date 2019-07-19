using System;
using System.Linq;
using System.Text;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    public class ByteWriter
    {
        protected byte[] _data;
        public byte[] Data => _data;

        protected int _writeIndex;

        public ByteWriter()
        {
            
        }
        
        public ByteWriter(int size)
        {
            Init(size);
        }

        public void Init(int size)
        {
            _data = new byte[size];
            _writeIndex = 0;
        }
        
        /// <returns>The data of this buffer up to its current write index.</returns>
        public byte[] ToArray()
        {
            byte[] arr = new byte[_writeIndex + 1];
            Array.Copy(_data, arr, arr.Length);
            return arr;
        }
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(bool value) => sizeof(bool);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(byte value) => sizeof(byte);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(char value) => sizeof(char);
        
        /*/// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(decimal value) => sizeof(int) * 5;*/
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(double value) => sizeof(double);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(float value) => sizeof(float);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(int value) => sizeof(int);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(long value) => sizeof(long);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(sbyte value) => sizeof(sbyte);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(short value) => sizeof(short);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(string value) => SizeOf(value.Length) + Encoding.UTF8.GetByteCount(value);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(uint value) => sizeof(uint);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(ulong value) => sizeof(ulong);
        
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public int SizeOf(ushort value) => sizeof(ushort);

        /// <returns>Size in bytes that the (multi-dimensional) array will occupy in the buffer.</returns>
        public int SizeOf(Array value)
        {
            int size = SizeOf(value.Length);
            foreach (var i in value)
            {
                int s;
                if ((s = TrySizeOf(i)) < 0)
                {
                    return -1;
                }

                size += s;
            }

            return size;
        }

        /// <summary>
        /// Will attempt to return the size of the given value, if known.
        /// </summary>
        /// <returns>Size in bytes that the value will occupy in the buffer.</returns>
        public virtual int TrySizeOf(object value)
        {
            switch (value)
            {
                case bool a:
                    return SizeOf(a);
                case byte b:
                    return SizeOf(b);
                case char c:
                    return SizeOf(c);
                //case decimal d:
                    //return SizeOf(d);
                case double e:
                    return SizeOf(e);
                case float f:
                    return SizeOf(f);
                case int g:
                    return SizeOf(g);
                case long h:
                    return SizeOf(h);
                case sbyte i:
                    return SizeOf(i);
                case short j:
                    return SizeOf(j);
                case string k:
                    return SizeOf(k);
                case uint l:
                    return SizeOf(l);
                case ulong m:
                    return SizeOf(m);
                case ushort n:
                    return SizeOf(n);
                case Array o:
                    return SizeOf(o);
                default:
                    return -1;
            }
        }
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(byte value)
        {
            if (_writeIndex > _data.Length)
            {
                Logger.Error("Attempting to exceed buffer limit! Size: " + _data.Length + " vs overflowing: " + _writeIndex);
                return false;
            }

            _data[_writeIndex++] = value;
            return true;
        }
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(byte[] value)
        {
            if (_writeIndex + value.Length > _data.Length)
            {
                Logger.Error("Attempting to exceed buffer limit! Size: " + _data.Length + " vs overflowing: " + _writeIndex + value.Length);
                return false;
            }

            foreach (var @byte in value)
            {
                _data[_writeIndex++] = @byte;
            }

            return true;
        }
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(bool value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(char value) => Write(BitConverter.GetBytes(value));
        
        //public bool Write(decimal value) => Write(decimal.GetBits(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(double value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(float value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(int value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(long value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(sbyte value) => Write((byte)(value + Math.Abs(sbyte.MinValue)));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(short value) => Write(BitConverter.GetBytes(value));

        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(string value)
        {
            return Write(value.Length) && Write(Encoding.UTF8.GetBytes(value));
        }
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(uint value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(ulong value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given value to the buffer, if in range.
        /// </summary>
        public bool Write(ushort value) => Write(BitConverter.GetBytes(value));
        
        /// <summary>
        /// Writes the given array to the buffer, if in range and the value type is known. Supports multi-dimensional arrays.
        /// </summary>
        public bool Write(Array value)
        {
            int startIndex = _writeIndex; // Revert to start if something goes wrong while writing
            
            Write(value.Length);
            foreach (var i in value)
            {
                if (!TryWrite(i))
                {
                    _writeIndex = startIndex;
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Will attempt to write the given value to the buffer, if known
        /// </summary>
        public virtual bool TryWrite(object value)
        {
            switch (value)
            {
                case bool a:
                    return Write(a);
                case byte b:
                    return Write(b);
                case char c:
                    return Write(c);
                //case decimal d:
                    //return Write(d);
                case double e:
                    return Write(e);
                case float f:
                    return Write(f);
                case int g:
                    return Write(g);
                case long h:
                    return Write(h);
                case sbyte i:
                    return Write(i);
                case short j:
                    return Write(j);
                case string k:
                    return Write(k);
                case uint l:
                    return Write(l);
                case ulong m:
                    return Write(m);
                case ushort n:
                    return Write(n);
                case Array o:
                    return Write(o);
                default:
                    return false;
            }
        }

        public static bool IsSupported(object value) => value is bool || value is byte || value is char || /*value is decimal ||*/
                                              value is double || value is float || value is int || value is long ||
                                              value is sbyte || value is short || value is string || value is uint ||
                                              value is ulong || value is ushort || value is Array;
    }
}