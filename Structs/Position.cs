using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            return (((long)pos.X & 0x3FFFFFF) << 38) | (((long)pos.Y & 0xFFF) << 26) | ((long)pos.Z & 0x3FFFFFF);
        }
        public static Position Decode(long value)
        {
            int x = (int)(value >> 38);
            int y = (int)((value >> 26) & 0xFFF);
            int z = (int)(value << 38 >> 38);

            return new Position(x, y, z);
        }

        public static double GetDistance(Position from, Position to)
        {
            double distance = Math.Sqrt(
                Math.Pow((to.X - from.X), 2) +
                Math.Pow((to.Y - from.Y), 2) +
                Math.Pow((to.Z - from.Z), 2)
                );

            return distance;
        }

        public Position GetXZ()
        {
            return new Position(this.X, 0, this.Z);
        }
        public Position GetXY()
        {
            return new Position(this.X, this.Y, 0);
        }
        public Position GetZY()
        {
            return new Position(0, this.Y, this.Z);
        }

        public static Position FromFace(Face face)
        {
            switch (face)
            {
                case Face.Bottom:
                    return new Position(0, -1, 0);
                case Face.Top:
                    return new Position(0, 1, 0);
                case Face.North:
                    return new Position(0, 0, -1);
                case Face.South:
                    return new Position(0, 0, 1);
                case Face.West:
                    return new Position(-1, 0, 0);
                case Face.East:
                    return new Position(1, 0, 0);
                default:
                    return new Position(0, 0, 0);
            }
        }

        public static implicit operator long(Position pos) => Encode(pos);
        public static implicit operator string(Position pos) => $"X: {pos.X} Y: {pos.Y} Z: {pos.Z}";

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
    }
}
