using Newtonsoft.Json;
using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Status
{
    internal class Ping : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public Ping(ClientHandler handler)
        {
            this.ClientHandler = handler;
            this.Version = handler.ClientVersion;

            PacketID = PacketHandler.Get(Version, PacketType.PING);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            return bm.GetLong();
        }

        public async void ReadWrite(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            long value = bm.GetLong();

            bm.SetPacketId((byte)PacketID);
            bm.AddBytes(bytes);

            await this.ClientHandler.FlushData(bytes, false);
        }

        public void Write()
        {
            BufferManager bm = new BufferManager();


            this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
