using SlimeCore.Entity;
using SlimeCore.Enums;
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

        public UpdateEntityRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.UPDATE_ENTITY_ROTATION);
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

            int angleYaw = (int)((player.CurrentPosition.Yaw / 360) * 256);
            int anglePitch = (int)((player.CurrentPosition.Pitch / 360) * 256);

            bm.AddVarInt(player.EntityID);
            bm.AddByte((byte)angleYaw);
            bm.AddByte((byte)anglePitch);
            bm.AddBool(player.IsOnGround);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
