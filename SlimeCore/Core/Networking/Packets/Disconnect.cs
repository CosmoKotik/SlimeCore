using Newtonsoft.Json;
using SlimeCore.Core.Enums;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class Disconnect
    {
        private int _packetID = 0x1A;
        private ClientHandler _handler;

        public Disconnect(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteString(JsonConvert.SerializeObject(new ChatMessage() { text="asdasdasd", bold=false }));
            _handler.Flush(_packetID);
        }
    }
}
