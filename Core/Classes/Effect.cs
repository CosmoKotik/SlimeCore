using SlimeCore.Enums;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Effect
    {
        public EffectType EffectID { get; set; }
        public Position Location { get; set; }
        public int Data { get; set; }
        public bool DisableRelativeVolume { get; set; }

        public Effect SetEffectID(EffectType type)
        { 
            this.EffectID = type;
            return this;
        }
        public Effect SetLocation(Position pos)
        {
            this.Location = pos;
            return this;
        }
        public Effect SetData(int data) 
        {
            this.Data = data;
            return this;
        }
        public Effect SetDisableRelativeVolume(bool disable)
        { 
            this.DisableRelativeVolume = disable;
            return this;
        }
    }
}
