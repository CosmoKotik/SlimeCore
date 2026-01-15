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
    public class EntityRelativeMovePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.ENTITY_RELATIVE_MOVE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public EntityRelativeMovePacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            MinecraftClient client = (MinecraftClient)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(client.EntityID);
            bm.WriteShort((short)(((client.WorldPosition.X * 32) - (client.PreviousWorldPosition.X * 32)) * 128));    //Change in X, Delta shit
            bm.WriteShort((short)(((client.WorldPosition.Y * 32) - (client.PreviousWorldPosition.Y * 32)) * 128));    //Change in Y, Delta shit
            bm.WriteShort((short)(((client.WorldPosition.Z * 32) - (client.PreviousWorldPosition.Z * 32)) * 128));    //Change in Z, Delta shit
            bm.WriteBool(client.IsOnGround);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);
            return this;
        }

        public object Write(object obj)
        {
            /*BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;*/
            throw new NotImplementedException();
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }
    }
}
