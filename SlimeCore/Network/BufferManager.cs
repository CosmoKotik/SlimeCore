using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    internal class BufferManager
    {
        private List<byte> _buffer = new List<byte>();

        #region Add/Insert

        public void AddInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void AddLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void AddString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            AddVarInt(bytes.Length);
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void AddBytes(byte[] value)
        {
            _buffer.Add((byte)value.Length);
            _buffer.AddRange(value);
        }
        public void AddByte(byte value)
        {
            //_buffer.Add((byte)1);
            _buffer.Add(value);
        }
        public void AddBool(bool value)
        {
            byte boolByte = value ? (byte)1 : (byte)0;
            _buffer.Add(boolByte);
        }

        public void AddVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        public void AddVarLong(long value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        public void SetPacketId(byte id)
        {
            _buffer = new List<byte>();

            if (_buffer.Count < 1)
                _buffer.Add(id);
            else
                _buffer[0] = id;
        }

        public void SetPacketUid(int puid)
        {
            _buffer.InsertRange(1, BitConverter.GetBytes(puid));
        }
        public static byte[] SetPacketUid(int puid, byte[] bytes)
        {
            List<byte> buffer = bytes.ToList();
            buffer.InsertRange(1, BitConverter.GetBytes(puid));
            return buffer.ToArray();
        }

        #endregion

        #region Get/Retreive
        public int GetPacketSize()
        {
            if (_buffer.Count > 0)
            {
                int size = ReadVarInt();
                //_buffer.RemoveAt(0);
                return size;
            }
            return -1;
        }
        public int GetPacketId()
        {
            if (_buffer.Count > 0)
            {
                int id = _buffer[0];
                _buffer.RemoveAt(0);
                return id;
            }
            return -1;
        }

        public int ReadVarInt()
        {
            var value = 0;
            var size = 0;
            int i = 0;
            int b;
            while (((b = _buffer[0]) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long. fuck you");
                }
                i++;
                _buffer.RemoveAt(0);
            }
            if (i == 0)
                _buffer.RemoveAt(0);
            return value | ((b & 0x7F) << (size * 7));
        }

        public int GetInt()
        {
            byte[] result = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 4);

            return BitConverter.ToInt32(result);
        }
        /*public int GetLong()
        {
            byte[] result = new byte[_buffer[0]];

            for (int i = 1; i < _buffer[0] + 1; i++)
            {
                result[i - 1] = _buffer[i];
            }

            _buffer.RemoveRange(0, _buffer[0] + 1);

            return BitConverter.ToInt32(result);
        }*/
        public long GetLong()
        {
            byte[] result = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 8);

            return BitConverter.ToInt64(result);
        }
        public string GetString()
        {
            byte[] result = new byte[_buffer[0]];

            for (int i = 1; i < (int)_buffer[0] + 1; i++)
            {
                result[i - 1] = _buffer[i];
            }

            _buffer.RemoveRange(0, (int)_buffer[0] + 1);

            return Encoding.UTF8.GetString(result);
        }
        public bool GetBool()
        {
            bool value = _buffer[0] != 0;
            _buffer.RemoveAt(0);
            return value;
        }

        #endregion

        public void SetBytes(byte[] bytes)
        {
            _buffer = bytes.ToList();
        }
        public void InsertBytes(byte[] bytes)
        {
            _buffer.AddRange(bytes);
        }
        public byte[] GetBytes()
        {
            return _buffer.ToArray();
        }
    }
}
