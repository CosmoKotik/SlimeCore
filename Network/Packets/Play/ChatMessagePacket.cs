using SlimeCore.Core;
using SlimeCore.Core.Chat;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class ChatMessagePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.CHAT_MESSAGE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public ChatMessagePacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            ChatMessage chatMessage = (ChatMessage)obj;
            string json = chatMessage.GetJson();

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteString(json);
            bm.WriteByte((byte)chatMessage.Position);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            ChatMessage chatMessage = (ChatMessage)obj;
            string json = chatMessage.GetJson();

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteString(json);
            bm.WriteByte((byte)chatMessage.Position);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
