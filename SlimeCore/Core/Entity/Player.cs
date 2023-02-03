using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Entity
{
    public class Player : Entity
    {
        public float AdditionalHearts = 0.0f;   //Index 15
        public int Score = 0;
        public byte DisplayedSkinPart = 0;
        public byte MainHand = 1;               //Default Right(Left: 0, Right: 1)

        public string Locate { get; set; }
        public string Uuid { get; set; }
        public string Username { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; } = 0;
        public bool ChatColors { get; set; } = true;

        public Vector3 Position { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }
    }
}
