using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class DestroyEntitiesPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.DESTROY_ENTITIES;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public DestroyEntitiesPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            Type objType = obj.GetType();
            IEntity[] entities;

            if (!objType.IsArray && !objType.GetInterfaces().Any(x => x.Equals(typeof(IEntity))))
                return this;

            if (!objType.IsArray)
                entities = new IEntity[1] { (IEntity)obj };
            else
                entities = (IEntity[])obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(entities.Length);
            for (int i = 0; i < entities.Length; i++)
                bm.WriteVarInt(entities[i].EntityID);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            IEntity[] entities;

            if (obj.GetType().Equals(typeof(IEntity)))
                entities = new IEntity[1] { (IEntity)obj };
            else
                entities = (IEntity[])obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteVarInt(entities.Length);
            for (int i = 0; i < entities.Length; i++)
                bm.WriteVarInt(entities[i].EntityID);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
