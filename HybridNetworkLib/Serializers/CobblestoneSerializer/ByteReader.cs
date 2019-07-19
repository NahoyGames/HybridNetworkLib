using System;
using System.Text;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    public class ByteReader
    {
        protected readonly byte[] _data;
        public byte[] Data => _data;

        protected int _readIndex;
        
        public virtual ByteWriter GetWriterEquivalent() => new ByteWriter();
        private ByteWriter _writer;

        public ByteReader(byte[] data)
        {
            _data = data;
            _writer = GetWriterEquivalent();
        }
        
        /// <returns>The value from the serialized data.</returns>
        public bool ReadBool(bool peek = false)
        {
            bool value = BitConverter.ToBoolean(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public byte ReadByte(bool peek = false)
        {
            byte value = _data[_readIndex];
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public char ReadChar(bool peek = false)
        {
            char value = BitConverter.ToChar(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /*/// <returns>The value from the serialized data.</returns>
        public decimal ReadDecimal(bool peek = false)
        {
            decimal value = new decimal((int[])ReadArray(typeof(int), peek));
            _readIndex += peek ? 0 : ByteWriter.SizeOf(value);
            return value;
        }*/
        
        /// <returns>The value from the serialized data.</returns>
        public double ReadDouble(bool peek = false)
        {
            double value = BitConverter.ToDouble(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public float ReadFloat(bool peek = false)
        {
            float value = BitConverter.ToSingle(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public int ReadInt(bool peek = false)
        {
            int value = BitConverter.ToInt32(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public long ReadLong(bool peek = false)
        {
            long value = BitConverter.ToInt64(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public sbyte ReadSByte(bool peek = false)
        {
            sbyte value = (sbyte)(_data[_readIndex] - Math.Abs(sbyte.MinValue));
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public short ReadShort(bool peek = false)
        {
            short value = BitConverter.ToInt16(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public string ReadString(bool peek = false)
        {
            int length = ReadInt(peek);
            string value = Encoding.UTF8.GetString(_data, _readIndex, length);
            _readIndex += peek ? 0 : _writer.SizeOf(value) - _writer.SizeOf(length);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public uint ReadUInt(bool peek = false)
        {
            uint value = BitConverter.ToUInt32(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public ulong ReadULong(bool peek = false)
        {
            ulong value = BitConverter.ToUInt64(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }
        
        /// <returns>The value from the serialized data.</returns>
        public ushort ReadUShort(bool peek = false)
        {
            ushort value = BitConverter.ToUInt16(_data, _readIndex);
            _readIndex += peek ? 0 : _writer.SizeOf(value);
            return value;
        }

        public Array ReadArray(Type arrayType, bool peek = false)
        {
            Type elementType = arrayType.GetElementType();
            
            Array arr = Array.CreateInstance(elementType, ReadInt(peek));
            
            for (int i = 0; i < arr.Length; i++)
            {
                arr.SetValue(TryRead(elementType, peek), i);
            }

            return arr;
        }

        /// <summary>
        /// Will attempt to call the correct Read[...]() method if type is known
        /// </summary>
        public virtual object TryRead(Type type, bool peek = false)
        {
            switch (true)
            {
                case bool _ when type == typeof(bool): return ReadBool(peek);
                case bool _ when type == typeof(byte): return ReadByte(peek);
                case bool _ when type == typeof(char): return ReadChar(peek);
                //case bool _ when type == typeof(decimal): return ReadDecimal(peek);
                case bool _ when type == typeof(double): return ReadDouble(peek);
                case bool _ when type == typeof(float): return ReadFloat(peek);
                case bool _ when type == typeof(int): return ReadInt(peek);
                case bool _ when type == typeof(long): return ReadLong(peek);
                case bool _ when type == typeof(sbyte): return ReadSByte(peek);
                case bool _ when type == typeof(short): return ReadShort(peek);
                case bool _ when type == typeof(string): return ReadString(peek);
                case bool _ when type == typeof(uint): return ReadUInt(peek);
                case bool _ when type == typeof(ulong): return ReadULong(peek);
                case bool _ when type == typeof(ushort): return ReadUShort(peek);
                case bool _ when type.IsArray: return ReadArray(type, peek);
                default:
                    Logger.Warn("Attempting to read an unsupported type!");
                    return null;
            }
        }
    }
}