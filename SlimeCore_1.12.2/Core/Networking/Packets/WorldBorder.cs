using SlimeCore.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class WorldBorder
    {
        private int _packetID = 0x38;
        private ClientHandler _handler;
        public WorldBorder(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteVarInt(0);
            _handler.WriteDouble(11111);
            _handler.Flush(_packetID);

            _handler.WriteVarInt(2);
            _handler.WriteDouble(0);
            _handler.WriteDouble(0);
            _handler.Flush(_packetID);
        }
    }
}
