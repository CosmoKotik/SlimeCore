using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SetEntityVelocity : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SetEntityVelocity(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_ENTITY_VELOCITY);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async Task Write() { }

        public async Task Write(Entity entity, Position velocity)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(entity.EntityID);
            bm.AddShort((short)velocity.PositionX);
            bm.AddShort((short)velocity.PositionY);
            bm.AddShort((short)velocity.PositionZ);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
