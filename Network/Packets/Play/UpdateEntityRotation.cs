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
    public class UpdateEntityRotation : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public UpdateEntityRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.UPDATE_ENTITY_ROTATION);
        }

        public UpdateEntityRotation Broadcast(bool includeSelf)
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
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            int angleYaw = (int)((player.CurrentPosition.Yaw / 360) * 256);
            int anglePitch = (int)((player.CurrentPosition.Pitch / 360) * 256);

            bm.AddVarInt(player.EntityID);
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
