using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SpawnPlayer : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SpawnPlayer(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SPAWN_PLAYER);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async void Write() { }

        public async void Write(Player player)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(player.EntityID);
            bm.AddUUID(player.UUID);
            bm.AddDouble(player.CurrentPosition.PositionX);
            bm.AddDouble(player.CurrentPosition.PositionY);
            bm.AddDouble(player.CurrentPosition.PositionZ);
            bm.AddByte((byte)player.CurrentPosition.Yaw);
            bm.AddByte((byte)player.CurrentPosition.Pitch);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
