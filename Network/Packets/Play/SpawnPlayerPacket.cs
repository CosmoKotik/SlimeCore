using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class SpawnPlayerPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.SPAWN_PLAYER;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public SpawnPlayerPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Write(object obj)
        {
            MinecraftClient client = (MinecraftClient)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(client.EntityID);
            bm.WriteUUID(client.UUID);
            bm.WriteDouble(client.WorldPosition.X);
            bm.WriteDouble(client.WorldPosition.Y);
            bm.WriteDouble(client.WorldPosition.Z);
            bm.WriteByte(client.Yaw);
            bm.WriteByte(client.Pitch);

            bm.WriteByte(0xff);     //Metadata, currently not implemented, sending end of md

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            MinecraftClient client = (MinecraftClient)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(client.EntityID);
            bm.WriteUUID(client.UUID);
            bm.WriteDouble(client.WorldPosition.X);
            bm.WriteDouble(client.WorldPosition.Y);
            bm.WriteDouble(client.WorldPosition.Z);
            bm.WriteByte(client.Yaw);
            bm.WriteByte(client.Pitch);

            bm.WriteByte(0xff);     //Metadata, currently not implemented, sending end of md

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }
    }
}
