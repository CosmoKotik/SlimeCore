using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Serverbound
{
    public enum SB_PlayPacketType
    {
        CLIENT_STATUS = 0x03,
        CLIENT_SETTINGS = 0x04,
        TELEPORT_CONFIRM = 0x00,
        KEEP_ALIVE = 0x0B,
        PLAYER = 0x0C,
        PLAYER_POSITION = 0x0D,
        PLAYER_POSITION_AND_LOOK = 0x0E,
        PLAYER_LOOK = 0x0F,
        ANIMATION = 0x1D,
        CHAT_MESSAGE = 0x02,
        PLAYER_DIGGING = 0x14,
        PLAYER_BLOCK_PLACEMENT = 0x1F
    }
}
