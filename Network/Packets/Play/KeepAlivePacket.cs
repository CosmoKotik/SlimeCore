using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    partial class KeepAlivePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.KEEP_ALIVE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public KeepAlivePacket(ClientHandler handler)
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

            long keepAliveID = (long)obj;

            bm.WriteLong(keepAliveID);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            Logger.Error("Sending keep alive packet", true);

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}