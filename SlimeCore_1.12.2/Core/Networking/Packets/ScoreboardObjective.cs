using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class ScoreboardObjective
    {
        private int _packetID = 0x42;
        private ClientHandler _handler;
        public ScoreboardObjective(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteString("XUY");
            _handler.WriteByte(0);
            _handler.Flush(_packetID);
        }
    }
}
