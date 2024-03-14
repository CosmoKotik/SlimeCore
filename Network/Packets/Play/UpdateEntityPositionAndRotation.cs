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
    internal class UpdateEntityPositionAndRotation : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public UpdateEntityPositionAndRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION);
        }

        public UpdateEntityPositionAndRotation Broadcast(bool includeSelf)
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

        public async Task Write(Player player)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            int angleYaw = (int)((player.CurrentPosition.Yaw / 360) * 256);
            int anglePitch = (int)((player.CurrentPosition.Pitch / 360) * 256);

            bm.AddVarInt(player.EntityID);
            bm.AddShort((short)((player.CurrentPosition.PositionX * 32 - player.PreviousPosition.PositionX * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionY * 32 - player.PreviousPosition.PositionY * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionZ * 32 - player.PreviousPosition.PositionZ * 32) * 128));
            bm.AddByte((byte)angleYaw);
            bm.AddByte((byte)anglePitch);
            bm.AddBool(player.IsOnGround);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
