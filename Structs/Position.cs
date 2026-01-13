using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Structs
{
    public struct Position
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Position(int x, int y, int z)
        { 
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static long Encode(Position pos)
        {
            return ((pos.X & 0x3FFFFFF) << 38) | ((pos.Y & 0xFFF) << 26) | (pos.Z & 0x3FFFFFF);
        }
        public static Position Decode(long value)
        {
            int x = (int)(value >> 38);
            int y = (int)((value >> 26) & 0xFFF);
            int z = (int)(value << 38 >> 38);

            return new Position(x, y, z);
        }

        public static implicit operator long(Position pos) => Encode(pos);
    }
}
