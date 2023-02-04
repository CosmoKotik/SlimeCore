using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Enums
{
    public class ClientPacketsEnum
    {
        public enum PlayPackets
        { 
            ClientSettings = 0x04,
            PlayerPositionAndLook = 0x0E,
            PlayerPosition = 0x0D,
            Player = 0x0C
        }
    }
}
