using SlimeCore.Core;
using SlimeCore.Core.Chunks;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class ChunkDataPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.CHUNK_DATA;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public ChunkDataPacket(ClientHandler handler)
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

            Chunk chunk = (Chunk)obj;

            bm.WriteBytes(chunk.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
