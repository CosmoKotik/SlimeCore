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
        KEEP_ALIVE = 0x0B
    }
}
