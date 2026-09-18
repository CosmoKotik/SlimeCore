using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Structs;

namespace SlimeCore.Network.Packets.Play
{
	public class ExplosionPacket : IPacket, IClientboundPacket
	{
        public int Id { get; set; } = (int)CB_PlayPacketType.EXPLOSION_PACKET;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public ExplosionPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }

        public object Write(object obj)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            Explosion explosion = (Explosion)obj;
            bm.WriteBytes(explosion.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            Explosion explosion = new Explosion();
            explosion.SetPosition(_handler.MC_Client.PreviousWorldPosition);
            explosion.SetRadius(999222);
            explosion.SetMotionPosition(Position.Zero);

            bm.WriteBytes(explosion.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }
    }
}

