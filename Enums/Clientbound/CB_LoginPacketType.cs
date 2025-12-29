using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Clientbound
{
    public enum CB_LoginPacketType
    {
        DISCONNECT = 0x00,
        ENCRYPTION_REQUEST = 0x01,
        LOGIN_SUCCESS = 0x02,
        SET_COMPRESSION = 0x03
    }
}
