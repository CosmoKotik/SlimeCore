using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class SetBlockDestroyStage : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SetBlockDestroyStage(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_BLOCK_DESTROY_STAGE);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async Task Write() { }

        public async Task Write(Player player, Position position, byte stage)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            long x = (long)position.PositionX;
            long y = (long)position.PositionY;
            long z = (long)position.PositionZ;

            long pos = ((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF);

            bm.AddVarInt(player.EntityID);
            bm.AddLong(pos);
            bm.AddByte(stage);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
