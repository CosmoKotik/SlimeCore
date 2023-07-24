using SlimeCore.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class HeldItemChange
    {
        private int _packetID = 0x3A;
        private ClientHandler _handler;
        public HeldItemChange(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteByte((byte)new Random().Next(0, 9));
            _handler.Flush(_packetID);
        }
    }
}
