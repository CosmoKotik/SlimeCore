using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets
{
    internal interface IServerboundPacket : IPacket
    {
        public object Read(object[] objs);
        //public object Read();

    }
}
