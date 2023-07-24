using Newtonsoft.Json;
using SlimeCore.Core.Entity;
using SlimeCore.Core.Enums;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class ChatMessage
    {
        private int _packetID = 0x0F;
        private ClientHandler _handler;

        public ChatMessage(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            Player player = _handler.CurrentPlayer;

            Chat chat = new Chat();
            chat.text = new Random().Next(0, int.MaxValue).ToString();

            _handler.WriteString(JsonConvert.SerializeObject(chat));
            _handler.WriteByte(0);
            _handler.Flush(_packetID);
        }
    }
}
