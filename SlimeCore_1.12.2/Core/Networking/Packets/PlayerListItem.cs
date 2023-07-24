using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class PlayerListItem
    {
        public int Action { get; set; } = 0;
        public int Latency { get; set; } = 0;

        private int _packetID = 0x2E;
        private ClientHandler _handler;
        public PlayerListItem(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write(ClientHandler handler = null)
        {
            if (handler == null)
                handler = _handler;

            Player player = _handler.CurrentPlayer;

            handler.WriteVarInt(Action);
            handler.WriteVarInt(1);
            handler.WriteUUID(new Guid(player.Uuid));

            switch (this.Action)
            {
                case 0: //Add player
                    handler.WriteString(player.Username);
                    handler.WriteVarInt(0);
                    handler.WriteVarInt((byte)player.Gamemode);
                    handler.WriteVarInt(0);
                    handler.WriteBool(false);
                    break;
                case 1: //Update gamemode
                    handler.WriteVarInt((byte)player.Gamemode);
                    break;
                case 2: //Update latency
                    handler.WriteVarInt(Latency);
                    break;
            }

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
                //if (!includeSelf)
                //{
                //    if (clientHandlers[i] != _handler)
                //        Write(clientHandlers[i]);
                //}
                //else
                //    Write(clientHandlers[i]);
            }
        }
    }
}
