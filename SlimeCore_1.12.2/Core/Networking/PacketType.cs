using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking
{
    public class PacketType
    {
        public enum PType 
        { 
            Null,
            Status,
            Login,
            Play
        }
    }
}
