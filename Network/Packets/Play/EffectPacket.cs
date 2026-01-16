using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class EffectPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.EFFECT;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public EffectPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            Effect effect = (Effect)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteInt((int)effect.EffectID);
            bm.WritePosition(effect.Location);
            bm.WriteInt(effect.Data);
            bm.WriteBool(effect.DisableRelativeVolume);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            Effect effect = (Effect)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteInt((int)effect.EffectID);
            bm.WritePosition(effect.Location);
            bm.WriteInt(effect.Data);
            bm.WriteBool(effect.DisableRelativeVolume);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
