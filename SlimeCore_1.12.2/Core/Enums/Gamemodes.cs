using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Enums
{
    public class Gamemodes
    {
        public enum Gamemode : byte
        { 
            Survival = 0,
            Creative = 1,
            Adventure = 2,
            Spectator = 3
        }
    }
}
