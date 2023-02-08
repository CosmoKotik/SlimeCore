using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    public class NibbleArray
    {
        public NibbleArray()
        {

        }
        public NibbleArray(int length)
        {
            Data = new byte[length / 2];
        }
        public byte[] Data { get; set; }
        public int Length
        {
            get { return Data.Length * 2; }
        }
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
    }
}
