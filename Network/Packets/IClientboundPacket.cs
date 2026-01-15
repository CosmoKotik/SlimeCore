using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets
{
    public interface IClientboundPacket : IPacket
    {
        public object Broadcast(bool includeSelf = false);
        public object Broadcast(object obj = null, bool includeSelf = false);

        public object Write(object obj);
        public object Write();
    }
}
