using SlimeCore.Core.Entity;
using SlimeCore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class KeepAlive
    {
        private int _packetID = 0x1F;
        private ClientHandler _handler;

        public KeepAlive(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            Player player = _handler.CurrentPlayer;

            _handler.WriteLong(new Random().NextInt64(0, long.MaxValue));
            _handler.Flush(_packetID);
        }

        public long Read()
        {
            return _handler.ReadLong();
        }
    }
}
