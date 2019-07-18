using System;
using System.Text;
using System.Collections.Generic;


namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    public class ByteBuffer : IDisposable
    {
        private readonly List<byte> _buff; // Bytes stored in this buffer are added here
        private byte[] _readBuff; // Used to convert an input byte array to other types
        private int _readIndex; // Stores where the counter is in the buffer
        private bool _buffUpdated = false; // Whether this buffer has been updated or not. If updated, goto next line

        #region Constructors
        public ByteBuffer()
        {
            _buff = new List<byte>();
            _readIndex = 0;
        }

        public ByteBuffer(byte[] data) : this()
        {
            Write(data);
        }

        public ByteBuffer(string data) : this()
        {
            Write(data);
        }

        public ByteBuffer(short data) : this()
        {
            Write(data);
        }

        public ByteBuffer(ushort data) : this()
        {
            Write(data);
        }

        public ByteBuffer(int data) : this()
        {
            Write(data);
        }

        public ByteBuffer(uint data) : this()
        {
            Write(data);
        }

        public ByteBuffer(long data) : this()
        {
            Write(data);
        }

        public ByteBuffer(ulong data) : this()
        {
            Write(data);
        }
        #endregion

        #region Instance Field Methods

        public long ReadIndex { get => _readIndex; }

        public byte[] ToArray(bool trimmed = true)
        {
            if (trimmed)
            {
                return _buff.GetRange(_readIndex, _buff.Count - _readIndex).ToArray();
            }

            return _buff.ToArray();
        }

        public int Count { get => _buff.Count; }
        public int Length { get => (Count - _readIndex); }

        #endregion


        #region Edit Methods

        public void Clear()
        {
            _buff.Clear();
            _readIndex = 0;
        }

        #endregion


        #region Write Buffer

        public void Write(byte[] value)
        {
            _buff.AddRange(value);
            _buffUpdated = true;
        }

        public void Write(byte value)
        {
            _buff.Add(value);
            _buffUpdated = true;
        }

        public void Write(short value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(ushort value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(int value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(uint value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(float value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(long value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(ulong value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            _buff.AddRange(BitConverter.GetBytes(value.Length)); // First specifies the length of the string so it doesn't mix with others that follow
            Write(Encoding.ASCII.GetBytes(value));
        }

        public void Write<T>(T value) where T : struct, IConvertible
        {
            Write((ushort)(IConvertible)value);
        }

        /*public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        public void Write(Quaternion value)
        {
            Write(new Vector4(value.x, value.y, value.z, value.w));
        }*/

        #endregion


        #region Read Buffer

        public int ReadInteger(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                int converted = BitConverter.ToInt32(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 4; // Int32 = 4 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public uint ReadUInteger(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                uint converted = BitConverter.ToUInt32(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 4; // Int32 = 4 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public byte[] ReadBytes(int length, bool peek = true)
        {
            if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
            {
                _readBuff = _buff.ToArray();
                _buffUpdated = false;
            }

            byte[] converted = _buff.GetRange(_readIndex, length).ToArray();

            if (peek)
            {
                _readIndex += length;
            }

            return converted;
        }


        public byte ReadByte(bool peek = true)
        {
            if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
            {
                _readBuff = _buff.ToArray();
                _buffUpdated = false;
            }

            byte converted = _buff[_readIndex];

            if (peek)
            {
                _readIndex += 1;
            }

            return converted;
        }


        public string ReadString(bool peek = true)
        {
            int length = ReadInteger(); // Reads out the length of the string

            if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
            {
                _readBuff = _buff.ToArray();
                _buffUpdated = false;
            }

            string converted = Encoding.ASCII.GetString(_readBuff, _readIndex, length);

            if (peek && Count > _readIndex)
            {
                if (converted.Length > 0)
                {
                    _readIndex += converted.Length;
                }
            }

            return converted;
        }


        public short ReadShort(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                short converted = BitConverter.ToInt16(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 2; // Int16 = 2 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public ushort ReadUShort(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                ushort converted = BitConverter.ToUInt16(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 2; // Int16 = 2 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public float ReadFloat(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                float converted = BitConverter.ToSingle(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 4; // Float = 4 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public long ReadLong(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                long converted = BitConverter.ToInt64(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 8; // Long = Int64 = 8 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public ulong ReadULong(bool peek = true)
        {
            if (Count > _readIndex)
            {
                if (_buffUpdated)    // Buff has been updated --> Not writing to it --> Read it out
                {
                    _readBuff = _buff.ToArray();
                    _buffUpdated = false;
                }

                ulong converted = BitConverter.ToUInt64(_readBuff, _readIndex);

                if (peek && Count > _readIndex)
                {
                    _readIndex += 8; // Long = Int64 = 8 bytes
                }

                return converted;
            }
            else
            {
                throw new Exception("Byte Buffer exceeded limit!");
            }
        }


        public T ReadEnum<T>(bool peek = true) where T : struct, IConvertible
        {
            return (T)(object)ReadUShort(peek);
        }


        /*public Vector2 ReadVector2(bool peek = true)
        {
            float x = ReadFloat(peek);
            float y = ReadFloat(peek);

            return new Vector2(x, y);
        }


        public Vector3 ReadVector3(bool peek = true)
        {
            float x = ReadFloat(peek);
            float y = ReadFloat(peek);
            float z = ReadFloat(peek);

            return new Vector3(x, y, z);
        }


        public Vector4 ReadVector4(bool peek = true)
        {
            float x = ReadFloat(peek);
            float y = ReadFloat(peek);
            float z = ReadFloat(peek);
            float w = ReadFloat(peek);

            return new Vector4(x, y, z, w);
        }


        public Quaternion ReadQuaternion(bool peek = true)
        {
            Vector4 vec4 = ReadVector4(peek);

            return new Quaternion(vec4.x, vec4.y, vec4.z, vec4.w);
        }*/

        #endregion


        #region IDisposable

        private bool _isDisposed = false; // Detects redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Clear();
                }

                _readIndex = 0;
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
