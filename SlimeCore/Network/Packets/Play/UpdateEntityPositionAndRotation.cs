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

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));

            short currentX = 0;
            short prevX = 0;
            short currentY = 0;
            short prevY = 0;
            short currentZ = 0;
            short prevZ = 0;

            bm.AddVarInt(0);
            bm.AddShort((short)((currentX * 32 - prevX * 32) * 128));
            bm.AddShort((short)((currentY * 32 - prevY * 32) * 128));
            bm.AddShort((short)((currentZ * 32 - prevZ * 32) * 128));
            bm.AddByte(0);
            bm.AddByte(0);
            bm.AddBool(false);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
