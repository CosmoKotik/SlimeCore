using SlimeCore.Entity;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class SetHeadRotation : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SetHeadRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_HEAD_ROTATION);
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

            int angle = (int)((player.CurrentPosition.Yaw / 360) * 256);
            bm.AddVarInt(player.EntityID);
            bm.AddByte((byte)angle);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
