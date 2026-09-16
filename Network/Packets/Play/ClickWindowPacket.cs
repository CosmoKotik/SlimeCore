using System;
using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Serverbound;
using SlimeCore.Network.Queue;

namespace SlimeCore.Network.Packets.Play
{
	public class ClickWindowPacket: IPacket, IServerboundPacket
	{
        public int Id { get; set; } = (int)SB_PlayPacketType.CLICK_WINDOW;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public ClickWindowPacket(ClientHandler handler)
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

        public object Read(object[] objs)
        {
            byte[] bytes = objs[0] as byte[];

            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            ClickWindow window_click = new ClickWindow()
                .SetWindowID(bm.ReadByte())
                .SetSlot(bm.ReadShort())
                .SetButton(bm.ReadByte())
                .SetActionNumber(bm.ReadShort())
                .SetMode((byte)bm.ReadVarInt())
                .SetClickedItem();
        }
    }
}

