using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entity
{
    public class Player
    {
        public int EntityID { get; set; }
        public string Username { get; set; }
        public bool IsHardcore { get; set; }
        public byte Gamemode { get; set; }
        public byte PreviousGamemode { get; set; }
        public int DimensionCount { get; set; }
        public string[] DimensionNames { get; set; }

        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColored { get; set; }
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public Guid UUID { get; set; }

        public Position CurrentPosition { get; set; }
        public Position PreviousPosition { get; set; }

        public Player()
        { 
            CurrentPosition = new Position();
            PreviousPosition = new Position();

            Gamemode = 0x01;
            PreviousGamemode = 0xFF;
        }
    }
}
