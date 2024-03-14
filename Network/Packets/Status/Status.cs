using Newtonsoft.Json;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Status
{
    public class Status : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public Status(ClientHandler handler)
        {
            this.ClientHandler = handler;
            this.Version = handler.ClientVersion;

            PacketID = PacketHandler.Get(Version, PacketType.STATUS);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async Task Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            StatusResponse sr = new StatusResponse()
            {
                version = new StatusVersion() { name = "Slime", protocol = (int)ClientHandler.ClientVersion },
                players = new Players() { max = 0, online = 120/*,sample = new Player[0]*/ },
                description = new Description() { text = "SlimeCore" },
                //favicon = "",
                enforcesSecureChat = false,
                previewsChat = false
            };

            bm.AddString(JsonConvert.SerializeObject(sr));

            QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
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
