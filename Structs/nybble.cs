using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Structs
{
    public struct nybble
    {
        public readonly int Value;

        public nybble(int value) 
        { 
            this.Value = value; 
        }

        public byte GetHalfByte()
        {
            bool isOdd = (Value & 1) == 0;  //Check if value is even or odd

            byte result = 0x00;

            //If odd then high bits, if even then low bits. fucking what, thank you daddy notch for complicating the shit out of this
            if (isOdd)
                result = (byte)(Value >> 4 & 0xF);
            else
                result = (byte)(Value & 0xF);

            return result;
        }

        public static implicit operator int(nybble nib) => nib.Value;
        public static implicit operator byte(nybble nib) => nib.GetHalfByte();
        public static implicit operator nybble(int value) => new nybble(value);
    }
}
