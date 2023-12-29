using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entity
{
    public class Position
    {
        public double PositionX { get; set; } = 0;
        public double PositionY { get; set; } = 0;
        public double PositionZ { get; set; } = 0;
        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;

        public Position() { }

        public Position(double X, double Y, double Z, float yaw, float pitch)
        { 
            this.PositionX = X;
            this.PositionY = Y;
            this.PositionZ = Z;
            this.Yaw = yaw;
            this.Pitch = pitch;
        }

        public Position(double X, double Y, double Z)
        {
            this.PositionX = X;
            this.PositionY = Y;
            this.PositionZ = Z;
        }

        public static Position operator *(Position pos1, Position pos2)
        {
            return new Position()
            {
                PositionX = pos1.PositionX * pos2.PositionX,
                PositionY = pos1.PositionY * pos2.PositionY,
                PositionZ = pos1.PositionZ * pos2.PositionZ,
                Yaw = pos1.Yaw * pos2.Yaw,
                Pitch = pos1.Pitch * pos2.Pitch
            };
        }

        public static Position operator /(Position pos1, Position pos2)
        {
            return new Position()
            {
                PositionX = pos1.PositionX / pos2.PositionX,
                PositionY = pos1.PositionY / pos2.PositionY,
                PositionZ = pos1.PositionZ / pos2.PositionZ,
                Yaw = pos1.Yaw / pos2.Yaw,
                Pitch = pos1.Pitch / pos2.Pitch
            };
        }

        public static Position operator +(Position pos1, Position pos2)
        {
            return new Position()
            {
                PositionX = pos1.PositionX + pos2.PositionX,
                PositionY = pos1.PositionY + pos2.PositionY,
                PositionZ = pos1.PositionZ + pos2.PositionZ,
                Yaw = pos1.Yaw + pos2.Yaw,
                Pitch = pos1.Pitch + pos2.Pitch
            };
        }

        public static Position operator -(Position pos1, Position pos2)
        {
            return new Position()
            {
                PositionX = pos1.PositionX - pos2.PositionX,
                PositionY = pos1.PositionY - pos2.PositionY,
                PositionZ = pos1.PositionZ - pos2.PositionZ,
                Yaw = pos1.Yaw - pos2.Yaw,
                Pitch = pos1.Pitch - pos2.Pitch
            };
        }

        public Position Clone()
        {
            return new Position()
            {
                PositionX = this.PositionX,
                PositionY = this.PositionY,
                PositionZ = this.PositionZ,
                Yaw = this.Yaw,
                Pitch = this.Pitch
            };
        }
    }
}
