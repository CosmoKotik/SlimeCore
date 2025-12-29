using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Serverbound
{
    public enum HandshakePacketType
    {
        HANDSHAKE = 0x00,
        HANDSHAKE_LEGACY = 0xFE
    }
}
