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
    internal class SpawnEntity : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public SpawnEntity(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SPAWN_ENTITY);
        }

        public SpawnEntity Broadcast(bool includeSelf)
        {
            _includeSelf = includeSelf;
            _broadcast = true;

            return this;
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async Task Write() { }

        public async Task Write(Entity entity)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            int headAngle = (int)(entity.CurrentPosition.Yaw - (360 * (int)(entity.CurrentPosition.Yaw / 360)));

            bm.AddVarInt(entity.EntityID);
            bm.AddUUID(entity.UUID);
            bm.AddVarInt((int)entity.EntityType);
            bm.AddDouble(entity.CurrentPosition.PositionX);
            bm.AddDouble(entity.CurrentPosition.PositionY);
            bm.AddDouble(entity.CurrentPosition.PositionZ);
            bm.AddByte((byte)entity.CurrentPosition.Pitch);
            bm.AddByte((byte)entity.CurrentPosition.Yaw);
            bm.AddByte((byte)headAngle);

            bm.AddVarInt(0);

            bm.AddShort(0);
            bm.AddShort(0);
            bm.AddShort(0);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
