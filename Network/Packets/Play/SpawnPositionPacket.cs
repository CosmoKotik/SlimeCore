using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class SpawnPositionPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.SPAWN_POSITION;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public SpawnPositionPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Write(object obj)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteLong((Position)obj);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            Position pos = new Position(0, 0, 0);

            bm.WriteLong(pos);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}
