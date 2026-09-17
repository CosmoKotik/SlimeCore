using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;

namespace SlimeCore.Network.Packets.Play
{
	public class OpenWindowPacket: IPacket, IClientboundPacket
	{
        public int Id { get; set; } = (int)CB_PlayPacketType.OPEN_WINDOW;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public OpenWindowPacket(ClientHandler handler)
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
            bm.SetPacketId((byte)Id);

            Window window = (Window)obj;
            bm.WriteBytes(window.GetBytes(), false);
            
            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            Window test_window = new Window()
                .SetWindowID(1)
                .SetWindowType("crafting_table")
                .SetWindowTitle("")
                .SetNumsOfSlots((byte)0);

            bm.WriteBytes(test_window.GetBytes(), false);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }
    }
}

