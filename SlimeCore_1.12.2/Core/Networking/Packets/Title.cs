using Newtonsoft.Json;
using SlimeCore.Core.Utils;
using SlimeCore.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class Title
    {
        private int _packetID = 0x48;
        private ClientHandler _handler;
        public Title(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteVarInt(0);
            Chat chat = new Chat();
            chat.text = "§dPIZDA §f!= §bZALUPA";

            _handler.WriteString(JsonConvert.SerializeObject(chat));
            _handler.Flush(_packetID);
        }
    }
}
