using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Enums.Serverbound;
using SlimeCore.Network.Queue;

namespace SlimeCore.Network.Packets.Play
{
	public class ConfirmTransactionPacket: IPacket, IClientboundPacket, IServerboundPacket
	{
        public int Id { get; set; } = (int)CB_PlayPacketType.PLAYER_POSITION_AND_LOOK;

        public int CB_ID { get; set; } = (int)CB_PlayPacketType.CONFIRM_TRANSACTION;
        public int SB_ID { get; set; } = (int)SB_PlayPacketType.CONFIRM_TRANSACTION;

        public Version Version { get; set; }

        private ClientHandler _handler;

        public ConfirmTransactionPacket(ClientHandler handler)
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
            bm.SetPacketId((byte)CB_ID);

            ConfirmTransaction transaction = (ConfirmTransaction)obj;
            bm.WriteBytes(transaction.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            throw new NotImplementedException();
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)CB_ID);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }

        public object Read(object[] objs)
        {
            throw new NotImplementedException();
        }
    }
}

