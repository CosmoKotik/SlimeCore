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

            Player player = _handler.CurrentPlayer;

            handler.WriteVarInt((byte)player.EntityID);
            handler.WriteDouble((byte)player.Position.X);
            handler.WriteDouble((byte)player.Position.Y);
            handler.WriteDouble((byte)player.Position.Z);
            handler.WriteByte(Convert.ToByte(player.Yaw));
            handler.WriteByte(Convert.ToByte(player.Pitch));
            handler.WriteBool(player.OnGround);

            handler.Flush(_packetID);
        }

        public void Broadcast(bool includeSelf = false)
        {
            List<ClientHandler> clientHandlers = new List<ClientHandler>(_handler.ServerManager.ClientHandlers);
            if (!includeSelf)
                clientHandlers.Remove(_handler);
            for (int i = 0; i < clientHandlers.Count; i++)
            {
                Write(clientHandlers[i]);
            }
        }
    }
}
