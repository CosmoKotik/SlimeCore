using Newtonsoft.Json;
using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Login
{
    public class LoginStartPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_LoginPacketType.LOGIN_SUCCESS;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public LoginStartPacket(ClientHandler handler)
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

            string uuid = Guid.NewGuid().ToString();
            
            bm.WriteString(uuid);

            bm.WriteString((string)obj);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
