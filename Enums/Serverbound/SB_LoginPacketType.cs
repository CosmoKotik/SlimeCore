using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Serverbound
{
    public enum SB_LoginPacketType
    {
        LOGIN_START = 0x00,
        ENCRYPTION_RESPONSE = 0x01
    }
}
