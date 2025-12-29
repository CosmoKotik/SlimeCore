using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Structs
{
    public struct varint
    {
        public readonly int Length;
        private readonly int _value;

        public varint(int input) 
        {
            this._value = input;

            int i = 0;
            while (true) 
            {
                if ((input & ~0x7F) == 0)
                {
                    i++;
                    break;
                }
                i++;
                input = (int)(((uint)input) >> 7);
            }
            this.Length = i;
        }

        public static byte[] ToBytes(int value)
        {
            List<byte> result = new List<byte>();
            while (true)
            {
                if ((value & ~0x7F) == 0)
                {
                    result.Add((byte)value);
                    return result.ToArray();
                }

                result.Add((byte)((value & 0x7F) | 0x80));
                value = (int)(((uint)value) >> 7);
            }
        }

        public byte[] ToBytes()
        {
            int value = _value;

            List<byte> result = new List<byte>();
            while (true)
            {
                if ((value & ~0x7F) == 0)
                {
                    result.Add((byte)value);
                    return result.ToArray();
                }

                result.Add((byte)((value & 0x7F) | 0x80));
                value = (int)(((uint)value) >> 7);
            }
        }

        public static implicit operator int(varint vi) => vi._value;
        public static implicit operator varint(int vi) => new varint(vi);
    }
}
