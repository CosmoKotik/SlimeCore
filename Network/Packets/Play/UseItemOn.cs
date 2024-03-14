using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class UseItemOn : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public int Hand { get; set; }
        public Position Position { get; set; }
        public Direction Face { get; set; }
        public float CursorPosX { get; set; }
        public float CursorPosY { get; set; }
        public float CursorPosZ { get; set; }
        public bool InsideBlock { get; set; }
        public int Sequence { get; set; }

        public UseItemOn(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.USE_ITEM_ON);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            this.Hand = bm.ReadVarInt();
            long pos = bm.GetLong();

            double x = pos >> 38;
            double y = pos << 52 >> 52;
            double z = pos << 26 >> 38;

            this.Position = new Position(x, y, z);

            this.Face = (Direction)bm.GetByte();
            this.CursorPosX = bm.GetFloat();
            this.CursorPosY = bm.GetFloat();
            this.CursorPosZ = bm.GetFloat();
            this.InsideBlock = bm.GetBool();
            this.Sequence = bm.ReadVarInt();

            return this;
        }

        public async Task Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
