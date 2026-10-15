using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;

namespace SlimeCore.Network.Packets.Play
{
	public class BossBarPacket: IPacket, IClientboundPacket
	{
        public int Id { get; set; } = (int)CB_PlayPacketType.BOSS_BAR;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public BossBarPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public BossBarPacket()
        {
            throw new MissingFieldException();
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

            BossBar bar = new BossBar()
                .CreateUUID()
                .CreateAddAction("bar", 1, 6, 4, 0x1);

            bm.WriteBytes(bar.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }
    }
}

