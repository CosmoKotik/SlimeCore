using SlimeCore.Entity;
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

        public async void Write() { }

        public async void Write(Player player)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            int angleYaw = (int)((player.CurrentPosition.Yaw / 360) * 256);
            int anglePitch = (int)((player.CurrentPosition.Pitch / 360) * 256);

            bm.AddVarInt(player.EntityID);
            bm.AddShort((short)((player.CurrentPosition.PositionX * 32 - player.PreviousPosition.PositionX * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionY * 32 - player.PreviousPosition.PositionY * 32) * 128));
            bm.AddShort((short)((player.CurrentPosition.PositionZ * 32 - player.PreviousPosition.PositionZ * 32) * 128));
            bm.AddByte((byte)angleYaw);
            bm.AddByte((byte)anglePitch);
            bm.AddBool(player.IsOnGround);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
