using Newtonsoft.Json;
using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Login
{
    internal class LoginSuccess : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public LoginSuccess(ClientHandler handler)
        {
            this.ClientHandler = handler;
            this.Version = handler.ClientVersion;

            //PacketID = PacketHandler.Get(Version, PacketType.LOGIN_SUCCESS);
            PacketID = 0x02;
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async void Write() { }

        public async void Write(Player player)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            //bm.AddString("c4b13b59042c4a82bed5d5eaf124036a");
            //bm.AddString(GetResponseString("_CosmoKotik_"));
            //bm.AddULong(1);
            //bm.AddULong(1);
            bm.AddUUID(player.UUID);
            bm.AddString(player.Username);
            bm.AddVarInt(0);

            QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());

            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        private string GetResponseString(string username)
        {
            var httpClient = new HttpClient();

            string url = "https://api.mojang.com/users/profiles/minecraft/" + username;
            var response = httpClient.GetAsync(url).Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(url + "  :    " + contents);
            mcuuid mu = JsonConvert.DeserializeObject<mcuuid>(contents);

            return new Guid(mu.id).ToString();
        }

        internal class mcuuid
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}
