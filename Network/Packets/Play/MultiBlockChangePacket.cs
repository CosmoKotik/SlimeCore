using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Structs;

namespace SlimeCore.Network.Packets.Play
{
	public class MultiBlockChangePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.MULTI_BLOCK_CHANGE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public MultiBlockChangePacket(ClientHandler handler)
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

        Random rnd = new Random();

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            MultiBlock blockChange = new MultiBlock()
                .SetChunk(0, 1)
                .AddRecord(
                    new Position(rnd.Next(0, 15), 80, rnd.Next(0, 15)),
                    (rnd.Next(0, 30) << 4) | (0 & 15));

            bm.WriteBytes(blockChange.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }
    }
}
