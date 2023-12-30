using SlimeCore.Entity;
using SlimeCore.Enums;
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

        public UpdateEntityPositionAndRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async void Write() { }

        public async void Write(Player player)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            bm.AddVarInt(player.EntityID);
            bm.AddShort((short)((player.CurrentPosition.PositionX * 32 - player.PreviousPosition.PositionX * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionY * 32 - player.PreviousPosition.PositionY * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionZ * 32 - player.PreviousPosition.PositionZ * 32) * 128));
            bm.AddByte((byte)player.CurrentPosition.Yaw);
            bm.AddByte((byte)player.CurrentPosition.Pitch);
            bm.AddBool(player.IsOnGround);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
