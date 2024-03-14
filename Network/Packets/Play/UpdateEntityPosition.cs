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
    internal class UpdateEntityPosition : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public UpdateEntityPosition(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.UPDATE_ENTITY_POSITION);
        }

        public UpdateEntityPosition Broadcast(bool includeSelf)
        {
            _includeSelf = includeSelf;
            _broadcast = true;

            return this;
        }

        public object Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async Task Write() { }

        public async Task Write(Entity entity)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            bm.AddVarInt(entity.EntityID);
            bm.AddShort((short)((entity.CurrentPosition.PositionX * 32 - entity.PreviousPosition.PositionX * 32) * 128));
            bm.AddShort((short)((entity.CurrentPosition.PositionY * 32 - entity.PreviousPosition.PositionY * 32) * 128));
            bm.AddShort((short)((entity.CurrentPosition.PositionZ * 32 - entity.PreviousPosition.PositionZ * 32) * 128));
            bm.AddBool(entity.IsOnGround);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
        }

        public async void Write(Entity entity, Position velocity)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            bm.AddVarInt(entity.EntityID);
            /*bm.AddShort((short)velocity.PositionX);
            bm.AddShort((short)velocity.PositionY);
            bm.AddShort((short)velocity.PositionZ);*/
            bm.AddShort((short)((entity.CurrentPosition.PositionX * 32 - (entity.CurrentPosition.PositionX - velocity.PositionX) * 32) * 128));
            bm.AddShort((short)((entity.CurrentPosition.PositionY * 32 - (entity.CurrentPosition.PositionY - velocity.PositionY) * 32) * 128));
            bm.AddShort((short)((entity.CurrentPosition.PositionZ * 32 - (entity.CurrentPosition.PositionZ - velocity.PositionZ) * 32) * 128));
            bm.AddBool(entity.IsOnGround);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
