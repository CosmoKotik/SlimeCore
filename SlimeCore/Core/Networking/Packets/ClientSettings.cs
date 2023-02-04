using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class ClientSettings
    {
        private int _packetID = 0x04;
        private ClientHandler _handler;

        public ClientSettings(ClientHandler handler)
        { 
            _handler = handler;
        }

        public void Read(byte[] bytes)
        {
            string locate = "";
            byte viewDistance = 0;
            int chatMode = 0;
            bool chatColors = false;
            byte displayedSkin = 0x00;
            byte mainHand = 1;

            for (int i = 3; i - 3 < bytes[2]; i++)
                locate += Convert.ToChar(bytes[i]);

            int offset = bytes[2] + 3;
            viewDistance = bytes[offset];
            chatMode = bytes[offset + 1];
            chatColors = (int)bytes[offset + 2] != 0;
            displayedSkin = bytes[offset + 3];
            mainHand = bytes[offset + 4];

            _handler.CurrentPlayer.Locate = locate;
            _handler.CurrentPlayer.ViewDistance = viewDistance;
            _handler.CurrentPlayer.ChatMode = chatMode;
            _handler.CurrentPlayer.ChatColors = chatColors;
            _handler.CurrentPlayer.DisplayedSkinPart = displayedSkin;
            _handler.CurrentPlayer.MainHand = mainHand;
        }
    }
}
