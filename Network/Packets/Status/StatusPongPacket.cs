using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Status
{
    public class StatusPongPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_StatusPacketType.STATUS_PONG;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public StatusPongPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public IClientboundPacket Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Write(object obj)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteLong((long)obj);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}