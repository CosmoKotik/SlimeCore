using SlimeCore.Core.Entity;
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
            Player player = _handler.CurrentPlayer;

            _handler.WriteInt(player.EntityID);
            _handler.WriteByte((byte)player.Gamemode);
            _handler.WriteInt((int)Dimension.Dimensions.Overworld);
            _handler.WriteByte((byte)_handler.ServerManager.Difficutly);
            _handler.WriteByte(Convert.ToByte(_handler.ServerManager.MaxPlayers));
            _handler.WriteString("default");
            _handler.WriteBool(true);
            _handler.Flush(_packetID);
        }
    }
}
