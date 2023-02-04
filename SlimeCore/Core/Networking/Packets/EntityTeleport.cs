using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class EntityTeleport
    {
        private int _packetID = 0x4C;
        private ClientHandler _handler;
        public EntityTeleport(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write(ClientHandler handler = null)
        {
            if (handler == null)
                handler = _handler;

            Player player = handler.CurrentPlayer;

            handler.WriteVarInt(player.EntityID);
            handler.WriteDouble(player.Position.X);
            handler.WriteDouble(player.Position.Y);
            handler.WriteDouble(player.Position.Z);
            handler.WriteByte(Convert.ToByte(player.Yaw));
            handler.WriteByte(Convert.ToByte(player.Pitch));
            handler.WriteBool(player.OnGround);

            handler.Flush(_packetID);
        }

        public void Broadcast()
        {
            List<ClientHandler> clientHandlers = new List<ClientHandler>(_handler.ServerManager.ClientHandlers);
            clientHandlers.Remove(_handler);
            for (int i = 0; i < clientHandlers.Count; i++)
            {
                if (clientHandlers[i] != _handler)
                {
                    Write(clientHandlers[i]);
                }
            }
        }
    }
}
