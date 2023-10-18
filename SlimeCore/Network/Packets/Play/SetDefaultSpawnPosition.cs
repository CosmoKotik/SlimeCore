using Newtonsoft.Json.Linq;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SetDefaultSpawnPosition : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SetDefaultSpawnPosition(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_DEFAULT_SPAWN_POSITION);
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

            int x = 5;
            int y = 10;
            int z = 15;

            long pos = (((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF));

            long xpos = ((x & 0x3FFFFFF) << 38);
            long ypos = ((z & 0x3FFFFFF) << 12);
            long zpos = (y & 0xFFF);

            //Console.WriteLine(pos);

            /*if (x >= 1 << 25) { x -= 1 << 26; }
            if (y >= 1 << 11) { y -= 1 << 12; }
            if (z >= 1 << 25) { z -= 1 << 26; }*/

            bm.AddBytes(BitConverter.GetBytes(pos), false);
            //bm.AddBytes(BitConverter.GetBytes(xpos), false);
            //bm.AddBytes(BitConverter.GetBytes(ypos), false);
            //bm.AddBytes(BitConverter.GetBytes(zpos), false);
            bm.AddFloat(0);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
