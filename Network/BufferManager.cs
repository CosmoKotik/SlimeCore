using SlimeCore.Structs;
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

        public void WriteInt(int value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void WriteLong(long value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //_buffer.Add((byte)bytes.Length);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            _buffer.AddRange(bytes);
        }
        public void WriteShort(short value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void WriteUShort(ushort value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //_buffer.Add((byte)bytes.Length);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            _buffer.AddRange(bytes);
        }
        public void WriteULong(ulong value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //_buffer.Add((byte)bytes.Length);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            _buffer.AddRange(bytes);
        }
        public void WriteDouble(double value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void WriteFloat(float value, bool isReversed = true)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (isReversed)
                bytes = bytes.Reverse().ToArray();
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void WriteString(string value, bool useUShort = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            if (!useUShort)
                WriteVarInt(bytes.Length);
            else
            {
                //AddUShort((ushort)bytes.Length);
                WriteByte((byte)(bytes.Length >> 8));
                WriteByte((byte)bytes.Length);
            }
            //AddUShort((ushort)bytes.Length);
            //_buffer.Add((byte)bytes.Length);
            _buffer.AddRange(bytes);
        }
        public void WriteUUID(Guid uuid)
        {
            /*byte[] uuidArray = uuid.ToByteArray();
            _buffer.AddRange(uuidArray);*/

            string suka = ToStringBigEndian(uuid);

            _buffer.AddRange(new Guid(suka).ToByteArray());
        }
        public void WriteBytes(byte[] value, bool includeLength = true)
        {
            if (includeLength)
                _buffer.Add((byte)value.Length);
            _buffer.AddRange(value);
        }
        public void WriteByte(byte value)
        {
            //_buffer.Add((byte)1);
            _buffer.Add(value);
        }
        public void WriteBool(bool value)
        {
            byte boolByte = value ? (byte)1 : (byte)0;
            _buffer.Add(boolByte);
        }

        public void WriteVarInt(int value)
        {
            /*while ((value & -128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)(((uint)value) >> 7);
            }
            _buffer.Add((byte)value);*/

            while (true)
            {
                if ((value & ~0x7F) == 0)
                {
                    _buffer.Add((byte)value);
                    return;
                }

                _buffer.Add((byte)((value & 0x7F) | 0x80));

                // Note: >>> means that the sign bit is shifted with the rest of the number rather than being left alone
                value = (int)(((uint)value) >> 7);
                //int asd = UnsignedRightShift(value, 7);
                //value = UnsignedRightShift(value, 7);
            }
        }

        public int UnsignedRightShift(int signed, int places)
        {
            unchecked // just in case of unusual compiler switches; this is the default
            {
                var unsigned = (uint)signed;
                unsigned >>= places;
                return (int)unsigned;
            }
        }

        public void WriteVarLong(long value)
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
                varint size = ReadVarInt();
                return size - size.Length;
            }
            return -1;
        }
        public int GetPacketId()
        {
            if (_buffer.Count > 0)
            {
                varint id = ReadVarInt();

                //int id = _buffer[0];
                //_buffer.RemoveAt(0);
                return id;
            }
            return -1;
        }

        /*public int ReadVarInt()
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
            _buffer.RemoveAt(0);
            return value | ((b & 0x7F) << (size * 7));
        }*/
        public varint ReadVarInt()
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
            _buffer.RemoveAt(0);
            return value | ((b & 0x7F) << (size * 7));
        }

        public int ReadInt(bool isReversed = true)
        {
            byte[] result = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 4);

            if (isReversed)
                result = result.Reverse().ToArray();

            return BitConverter.ToInt32(result);
        }
        public short ReadShort(bool isReversed = true)
        {
            byte[] result = new byte[2];

            for (int i = 0; i < 2; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 2);

            if (isReversed)
                result = result.Reverse().ToArray();

            return BitConverter.ToInt16(result);
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
        public long ReadLong(bool isReversed = true)
        {
            byte[] result = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 8);

            if (isReversed)
                result = result.Reverse().ToArray();

            return BitConverter.ToInt64(result);
        }
        public double ReadDouble(bool isReversed = true)
        {
            byte[] result = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 8);

            if (isReversed)
                result = result.Reverse().ToArray();

            return BitConverter.ToDouble(result);
        }
        public float ReadFloat(bool isReversed = true)
        {
            byte[] result = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 4);

            if (isReversed)
                result = result.Reverse().ToArray();

            return BitConverter.ToSingle(result);
        }
        public string ReadString()
        {
            int len = ReadVarInt();
            byte[] result = new byte[len];

            for (int i = 0; i < len; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, len);

            return Encoding.UTF8.GetString(result);
        }
        public bool ReadBool()
        {
            bool value = _buffer[0] != 0;
            _buffer.RemoveAt(0);
            return value;
        }
        public byte ReadByte()
        {
            byte value = _buffer[0];
            _buffer.RemoveAt(0);
            return value;
        }

        public Guid ReadUUID()
        {
            byte[] result = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                result[i] = _buffer[i];
            }

            _buffer.RemoveRange(0, 16);

            return new Guid(result);
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
        public byte[] GetBytesWithLength()
        {
            byte[] size = varint.ToBytes(_buffer.Count);
            
            _buffer.InsertRange(0, size);
            
            byte[] result = _buffer.ToArray();

            _buffer.RemoveRange(0, size.Length);

            return result;
        }
        public void RemoveRangeByte(int range)
        {
            _buffer.RemoveRange(0, range);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private string ToStringBigEndian(Guid guid)
        {
            // allocate enough bytes to store Guid ASCII string
            Span<byte> result = stackalloc byte[36];
            // get bytes from guid
            Span<byte> buffer = stackalloc byte[16];
            _ = guid.TryWriteBytes(buffer);
            int skip = 0;
            // iterate over guid bytes
            for (int i = 0; i < buffer.Length; i++)
            {
                // indices 4, 6, 8 and 10 will contain a '-' delimiter character in the Guid string.
                // --> leave space for those delimiters
                // we can check if i is even and i / 2 is >= 2 and <= 5 to determine if we are at one of those indices
                // 0xF...F if i is odd and 0x0...0 if i is even
                int isOddMask = -(i & 1);
                // 0xF...F if i / 2 is < 2 and 0x0...0 if i / 2 is >= 2
                int less2Mask = ((i >> 1) - 2) >> 31;
                // 0xF...F if i / 2 is > 5 and 0x0...0 if i / 2 is <= 5
                int greater5Mask = ~(((i >> 1) - 6) >> 31);
                // 0xF...F if i is even and 2 <= i / 2 <= 5 otherwise 0x0...0
                int skipIndexMask = ~(isOddMask | less2Mask | greater5Mask);
                // skipIndexMask will be 0xFFFFFFFF for indices 4, 6, 8 and 10 and 0x00000000 for all other indices
                // --> skip those indices
                skip += 1 & skipIndexMask;
                result[(2 * i) + skip] = ToHexCharBranchless(buffer[i] >>> 0x4);
                result[(2 * i) + skip + 1] = ToHexCharBranchless(buffer[i] & 0x0F);
            }
            // add dashes
            const byte dash = (byte)'-';
            result[8] = result[13] = result[18] = result[23] = dash;
            // get string from ASCII encoded guid byte array
            return Encoding.ASCII.GetString(result);
        }

        private byte ToHexCharBranchless(int b) =>
            // b + 0x30 for [0-9] if 0 <= b <= 9 and b + 0x30 + 0x27 for [a-f] if 10 <= b <= 15
            (byte)(b + 0x30 + (0x27 & ~((b - 0xA) >> 31)));
    }
}
