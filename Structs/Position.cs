using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Structs
{
    public struct Position
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Position(int x, int y, int z)
        { 
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Position(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static long Encode(Position pos)
        {
            return (((int)pos.X & 0x3FFFFFF) << 38) | (((int)pos.Y & 0xFFF) << 26) | ((int)pos.Z & 0x3FFFFFF);
        }
        public static Position Decode(long value)
        {
            int x = (int)(value >> 38);
            int y = (int)((value >> 26) & 0xFFF);
            int z = (int)(value << 38 >> 38);

            return new Position(x, y, z);
        }

        public static implicit operator long(Position pos) => Encode(pos);
        public static implicit operator string(Position pos) => $"X: {pos.X} Y: {pos.Y} Z: {pos.Z}";
    }
}
