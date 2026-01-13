using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Clientbound
{
    public enum CB_PlayPacketType
    {
        JOIN_GAME = 0x23,
        SPAWN_POSITION = 0x46,
        PLAYER_POSITION_AND_LOOK = 0x2F,
        CHUNK_DATA = 0x20,
        KEEP_ALIVE = 0x1F,
        WORLD_BORDER = 0x38
    }
}
