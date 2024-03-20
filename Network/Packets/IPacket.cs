using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets
{
    public interface IPacket
    {
        public int Id { get; set; }
        public Version Version { get; set; }
    }
}
