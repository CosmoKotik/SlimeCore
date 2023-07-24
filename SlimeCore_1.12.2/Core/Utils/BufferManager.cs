using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    internal class BufferManager
    {
        internal void WriteVarInt(int value, ref List<byte> buffer)
        {
            while ((value & 128) != 0)
            {
                buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            buffer.Add((byte)value);
        }
        internal void Write(byte[] value, ref List<byte> buffer)
        {
            buffer.AddRange(value);
        }
        internal void WriteInt128(BigInteger value, ref List<byte> buffer)
        {
            buffer.AddRange(value.ToByteArray());
        }

        internal void WriteInt(int value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt(uint value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt16(Int16 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt16(UInt16 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt32(Int32 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt32(UInt32 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt64(Int64 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt64(UInt64 value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteLong(long value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteBool(bool value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteByte(byte value, ref List<byte> buffer)
        {
            buffer.Add(value);
        }

        internal void WriteShort(short value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUShort(ushort value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteDouble(Double value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteFloat(float value, ref List<byte> buffer)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteString(string data, ref List<byte> bufferd, int val = -1)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            if (val == -1)
                WriteVarInt(buffer.Length, ref bufferd);
            else
                WriteVarInt(val, ref bufferd);
            bufferd.AddRange(buffer);
        }
    }
}
