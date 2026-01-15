using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class PlayerListItemPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.PLAYER_LIST_ITEM;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public PlayerListItemPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            PlayerListItem item = (PlayerListItem)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt((int)item.Action);
            bm.WriteVarInt(1);  //Sets numbers of players that will be affected
            bm.WriteUUID(item.UUID);

            switch (item.Action)
            {
                case Enums.PlayerListItemAction.ADD_PLAYER:
                    bm.WriteString(item.Username);
                    bm.WriteVarInt(0);      //Number of properties, currently not implemented
                    bm.WriteVarInt(item.Gamemode);
                    bm.WriteVarInt(item.Ping);
                    bm.WriteBool(item.HasDisplayName);    //Has display name, currently fuck you
                    break;
                case Enums.PlayerListItemAction.UPDATE_GAMEMODE:
                    bm.WriteVarInt(item.Gamemode);
                    break;
                case Enums.PlayerListItemAction.UPDATE_LATENCY:
                    bm.WriteVarInt(item.Ping);
                    break;
                case Enums.PlayerListItemAction.UPDATE_DISPLAY_NAME:
                    bm.WriteBool(item.HasDisplayName);

                    if (item.HasDisplayName)
                        bm.WriteString(item.DisplayName);
                    break;
                case Enums.PlayerListItemAction.REMOVE_PLAYER:
                    break;
            }

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);
            return this;
        }

        public object Write(object obj)
        {
            PlayerListItem item = (PlayerListItem)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt((int)item.Action);
            bm.WriteVarInt(1);  //Sets numbers of players that will be affected
            bm.WriteUUID(item.UUID);

            switch (item.Action)
            {
                case Enums.PlayerListItemAction.ADD_PLAYER:
                    bm.WriteString(item.Username);
                    bm.WriteVarInt(0);      //Number of properties, currently not implemented
                    bm.WriteVarInt(item.Gamemode);
                    bm.WriteVarInt(item.Ping);
                    bm.WriteBool(item.HasDisplayName);    //Has display name, currently fuck you
                    break;
                case Enums.PlayerListItemAction.UPDATE_GAMEMODE:
                    bm.WriteVarInt(item.Gamemode);
                    break;
                case Enums.PlayerListItemAction.UPDATE_LATENCY:
                    bm.WriteVarInt(item.Ping);
                    break;
                case Enums.PlayerListItemAction.UPDATE_DISPLAY_NAME:
                    bm.WriteBool(item.HasDisplayName);

                    if (item.HasDisplayName)
                        bm.WriteString(item.DisplayName);
                    break;
                case Enums.PlayerListItemAction.REMOVE_PLAYER:
                    break;
            }

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}