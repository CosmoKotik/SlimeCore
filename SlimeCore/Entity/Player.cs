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
        public bool IsHardcore { get; set; }
        public byte Gamemode { get; set; }
        public byte PreviousGamemode { get; set; }
        public int DimensionCount { get; set; }
        public string[] DimensionNames { get; set; }
    }
}
