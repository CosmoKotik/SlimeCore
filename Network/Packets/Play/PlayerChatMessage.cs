using SlimeCore.Entities;
using SlimeCore.Entities.Chat;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class PlayerChatMessage : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public PlayerChatMessage(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.PLAYER_CHAT_MESSAGE);
        }

        public PlayerChatMessage Broadcast(bool includeSelf)
        {
            _includeSelf = includeSelf;
            _broadcast = true;

            return this;
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async void Write() { }

        public async void Write(Player player, string message, long timestamp, long salt)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            //Header
            bm.AddUUID(player.UUID);    //Sender
            bm.AddVarInt(0);            //Index, idfk what it does
            bm.AddBool(false);          //Chat signature, fuck it

            //Body
            bm.AddString(message);                  //Message
            bm.AddLong(DateTimeOffset.Now.ToUnixTimeMilliseconds());   //Timestamp
            bm.AddLong(0);                          //Salt, fuck knows what that is

            //Previous messages
            bm.AddVarInt(0);        //fuck arrays

            //Other
            bm.AddBool(false);                                  //Unsigned content present
            bm.AddVarInt((int)ChatFilterType.PASS_THROUGH);     //Filter type

            //Network target
            bm.AddVarInt(0);    //Chat type, fuck that shit i aint tryin to figure out shit

            Chat netowrkName = new ChatFactory()
                .SetText(player.Username)
                .SetInsertion(player.Username)
                .SetClickEvent("suggest_command", $"/tell {player.Username} ")
                .SetHoverEvent("show_entity", "minecraft:player", player.UUID.ToString(), player.Username).Build();

            bm.AddString(netowrkName.BuildJson());
            bm.AddBool(false);

            //Console.WriteLine("zalupa: {0}", BitConverter.ToString(bm.GetBytes()).Replace("-", " ") + "   " + bm.GetBytes().Length);

            QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
