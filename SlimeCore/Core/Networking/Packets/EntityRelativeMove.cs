using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class EntityRelativeMove
    {
        private int _packetID = 0x26;
        private ClientHandler _handler;
        public EntityRelativeMove(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write(ClientHandler handler = null)
        {
            if (handler == null)
                handler = _handler;
            //var data = (((long)spawn.X & 0x3FFFFFF) << 38) | (((long)spawn.Y & 0xFFF) << 26) | ((long)spawn.Z & 0x3FFFFFF);

            Player player = _handler.CurrentPlayer;

            handler.WriteVarInt(player.EntityID);
            //_handler.WriteUUID(new Guid(Player.Uuid));
            //_handler.WriteString(player.Uuid);
            handler.WriteShort((short)((player.Position.X * 32 - player.OldPosition.X * 32) * 128));
            handler.WriteShort((short)((player.Position.Y * 32 - player.OldPosition.Y * 32) * 128));
            handler.WriteShort((short)((player.Position.Z * 32 - player.OldPosition.Z * 32) * 128));

            //_handler.WriteShort(0);
            handler.WriteBool(player.OnGround);

            //_handler.WriteLong(data);
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
