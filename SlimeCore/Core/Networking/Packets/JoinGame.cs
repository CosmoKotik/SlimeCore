using SlimeCore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class JoinGame
    {
        private int _packetID = 0x23;
        private ClientHandler _handler;

        public JoinGame (ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            _handler.WriteInt(0);
            _handler.WriteByte((byte)Gamemodes.Gamemode.Survival);
            _handler.WriteInt((int)Dimension.Dimensions.Overworld);
            _handler.WriteByte((byte)Difficulty.Difficulties.easy);
            _handler.WriteByte(100);
            _handler.WriteString("default");
            _handler.WriteBool(true);
            _handler.Flush(_packetID);
        }
    }
}
