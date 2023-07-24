using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Enums.Protocols
{
    public class Protocols : IProtocol
    {

    }

    public class Protocol760 : IProtocol
    {
        public override int HANDSHAKE { get; set; } = 0x01;
    }
}
