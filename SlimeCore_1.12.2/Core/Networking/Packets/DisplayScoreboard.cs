using Newtonsoft.Json;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class DisplayScoreboard
    {
        private int _packetID = 0x3B;
        private ClientHandler _handler;
        public DisplayScoreboard(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteByte(1);
            _handler.WriteString("XUY");
            _handler.Flush(_packetID);
        }
    }
}
