﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets
{
    public interface IClientboundPacket : IPacket
    {
        public IClientboundPacket Broadcast(bool includeSelf);

        public object Write(object obj);
        public object Write();
    }
}
