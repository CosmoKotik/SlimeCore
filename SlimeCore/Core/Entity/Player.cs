using SlimeCore.Core.Enums;
using SlimeCore.Core.Networking;
using SlimeCore.Core.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static SlimeCore.Core.Enums.Gamemodes;

namespace SlimeCore.Core.Entity
{
    public class Player : Entity
    {
        public float AdditionalHearts = 0.0f;   //Index 15
        public int Score = 0;
        public byte DisplayedSkinPart = 0;
        public byte MainHand = 1;               //Default Right(Left: 0, Right: 1)

        public ClientHandler Handler;

        public int EntityID { get; set; }
        public string Locate { get; set; }
        public string Uuid { get; set; } = "";
        public string Username { get; set; } = "";
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; } = 0;
        public bool ChatColors { get; set; } = true;

        public Gamemode Gamemode { get; set; } = Gamemodes.Gamemode.Survival;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public int Yaw { get; set; }
        public int Pitch { get; set; }
        public bool OnGround { get; set; }

        public void PositionAndLookChanged(Vector3 position, int yaw, int pitch, bool onGround)
        {
            this.Position = position;
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.OnGround = onGround;

            //new EntityTeleport(Handler).Broadcast();
        }

        public void PositionChanged(Vector3 position, bool onGround)
        {
            this.Position = position;
            this.OnGround = onGround;
        }
    }
}
