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
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            //int angle = (int)(player.CurrentPosition.Yaw - (360 * (int)(player.CurrentPosition.Yaw / 360)));
            //int angle = (int)(player.CurrentPosition.Yaw * (360 / 256));
            int angle = (int)((player.CurrentPosition.Yaw / 360) * 256);
            /*if (player.CurrentPosition.Yaw < 0)
                angle = 180 + ((int)player.CurrentPosition.Yaw - 180);
            else
                angle = (int)player.CurrentPosition.Yaw;
*/
            Console.WriteLine(angle);

            bm.AddVarInt(player.EntityID);
            bm.AddByte((byte)angle);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
