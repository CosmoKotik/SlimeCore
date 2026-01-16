using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class AnimationPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.ANIMATION;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public AnimationPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            EntityAnimation e_anim = (EntityAnimation)obj;
            int e_id = e_anim.EntityID;
            AnimationType animation = e_anim.Animation;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(e_id);
            bm.WriteByte((byte)animation);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            EntityAnimation e_anim = (EntityAnimation)obj;
            int e_id = e_anim.EntityID;
            AnimationType animation = e_anim.Animation;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(e_id);
            bm.WriteByte((byte)animation);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
