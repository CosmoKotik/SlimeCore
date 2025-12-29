using Newtonsoft.Json;
using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Status
{
    public class Status : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_StatusPacketType.STATUS_RESPONSE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public Status(ClientHandler handler) 
        {
            this._handler = handler;
        }

        public IClientboundPacket Broadcast(bool includeSelf)
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

            StatusResponse sr = new StatusResponse()
            {
                version = new StatusVersion() { name = "Slime", protocol = (int)_handler.ClientVersion },
                players = new Players() { max = 0, online = 120/*,sample = new Player[0]*/ },
                description = new Description() { text = "SlimeCore" },
                //favicon = "",
                enforcesSecureChat = false,
                previewsChat = false
            };

            bm.WriteString(JsonConvert.SerializeObject(sr));

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        private class StatusResponse
        {
            public StatusVersion version { get; set; }
            public Players players { get; set; }
            public Description description { get; set; }
            //public string favicon { get; set; }
            public bool enforcesSecureChat { get; set; }
            public bool previewsChat { get; set; }
        }

        private class StatusVersion
        {
            public string name { get; set; }
            public int protocol { get; set; }
        }

        private class Players
        {
            public int max { get; set; }
            public int online { get; set; }
            //public Player[] sample { get; set; }
        }

        private class Player
        {
            public string name { get; set; }
            public string id { get; set; }
        }
        private class Description
        {
            public string text { get; set; }
        }
    }
}
