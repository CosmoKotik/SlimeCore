using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using fNbt;
using fNbt.Serialization;

namespace SlimeCore.Core.Utils
{
    public class NibbleArray : INbtSerializable
    {
        public NibbleArray()
        {

        }
        public NibbleArray(int length)
        {
            Data = new byte[length / 2];
        }

        public byte[] Data { get; set; }

        [NbtIgnore]
        public int Length
        {
            get { return Data.Length * 2; }
        }

        [NbtIgnore]
        public byte this[int index]
        {
            get { return (byte)(Data[index / 2] >> ((index) % 2 * 4) & 0xF); }
            set
            {
                value &= 0xF;
                Data[index / 2] &= (byte)(0xF << ((index + 1) % 2 * 4));
                Data[index / 2] |= (byte)(value << (index % 2 * 4));
            }
        }

        public NbtTag Serialize(string tagName)
        {
            return new NbtByteArray(tagName, Data);
        }

        public void Deserialize(NbtTag value)
        {
            Data = value.ByteArrayValue;
        }
    }
}
