using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class PlayerAction : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public int Status { get; set; }
        public Position Position { get; set; }
        public byte Face { get; set; }
        public int Sequence { get; set; }

        public PlayerAction(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.PLAYER_ACTION);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            this.Status = bm.ReadVarInt();
            long pos = bm.GetLong();

            double x = pos >> 38;
            double y = pos << 52 >> 52;
            double z = pos << 26 >> 38;

            this.Position = new Position(x, y, z);

            this.Face = bm.GetByte();
            this.Sequence = bm.ReadVarInt();

            return this;
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
