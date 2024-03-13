using Newtonsoft.Json.Linq;
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

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async void Write(Position position, float angle)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            long x = (long)position.PositionX;
            long y = (long)position.PositionY;
            long z = (long)position.PositionZ;

            long pos = ((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF);

            bm.AddLong(pos);
            bm.AddFloat(angle);

            QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public void Write()
        {
            throw new NotImplementedException();
        }
    }
}
