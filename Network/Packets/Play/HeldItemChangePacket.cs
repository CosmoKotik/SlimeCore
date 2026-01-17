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
    public class HeldItemChangePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.HELD_ITEM_CHANGE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public HeldItemChangePacket(ClientHandler handler)
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
