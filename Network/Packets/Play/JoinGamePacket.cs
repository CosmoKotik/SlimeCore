using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class JoinGamePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.JOIN_GAME;
        public Version Version { get; set; }

        private ClientHandler _handler;

        private Configs _config;

        public JoinGamePacket(ClientHandler handler, Configs config)
        {
            this._handler = handler;
            this._config = config;
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Write(object obj)
        {
            throw new NotImplementedException();
        }

        public object Write(MinecraftClient client)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            //int uid = new Random().Next(int.MaxValue);
            bm.WriteInt(client.EntityID);   //Player Entity ID
            bm.WriteByte((byte)client.Gamemode);    //Gamemode
            bm.WriteInt(client.WorldDimension);     //Dimension
            bm.WriteByte(0);    //Difficulty
            bm.WriteByte(0);    //Max Players, in modern minecraft its ignored(at least in 1.12.2)
            bm.WriteString("default"); //level type
            bm.WriteBool(false);    //debug shit

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            int uid = new Random().Next(int.MaxValue);
            bm.WriteInt(uid);   //Player Entity ID
            bm.WriteByte(1);    //Gamemode
            bm.WriteInt(0);     //Dimension
            bm.WriteByte(0);    //Difficulty
            bm.WriteByte(0);    //Max Players, in modern minecraft its ignored(at least in 1.12.2)
            bm.WriteString("default"); //level type
            bm.WriteBool(false);    //debug shit

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}
