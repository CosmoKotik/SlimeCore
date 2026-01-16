using SlimeCore.Enums;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Particle
    {
        public ParticleType ParticleID { get; set; }
        public bool IsLongDistance { get; set; }        //from 256 to 65565 distance
        public Position Position { get; set; }
        public Position Offset { get; set; } = new Position();
        public float ParticleData { get; set; }         //Data of each particle, fuck knows what they meant by this
        public int ParticleCount { get; set; }
        public List<varint> Data { get; set; } = new List<varint>();             //Length depends on particle. "iconcrack" has length of 2, "blockcrack", "blockdust", and "fallingdust" have lengths of 1, the rest have 0. (wiki.vg)

        public Particle SetParticleID(ParticleType type)
        {
            this.ParticleID = type;
            return this;
        }
        public Particle SetIsLongDistance(bool value)
        {
            this.IsLongDistance = value;
            return this;
        }
        public Particle SetPosition(Position value) 
        {
            this.Position = value;
            return this;
        }
        public Particle SetOffset(Position value) 
        {
            this.Offset = value;
            return this;
        }
        public Particle SetParticleData(float value) 
        {
            this.ParticleData = value;
            return this;
        }
        public Particle SetParticleCount(int value) 
        {
            this.ParticleCount = value;
            return this;
        }
        public Particle AddData(int value) 
        {
            this.Data.Add(value);
            return this;
        }
    }
}
