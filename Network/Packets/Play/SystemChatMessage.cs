using SlimeCore.Entities;
using SlimeCore.Entities.Chat;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SystemChatMessage : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SystemChatMessage(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SYSTEM_CHAT_MESSAGE);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async void Write() { }

        public async void Write(Player player, string message)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            Chat netowrkName = new ChatFactory()
                .SetText(player.Username)
                .SetInsertion(player.Username)
                .SetClickEvent("suggest_command", $"/tell {player.Username} ")
                .SetHoverEvent("show_entity", "minecraft:player", player.UUID.ToString(), player.Username).Build();

            bm.AddString(netowrkName.BuildJson());
            bm.AddBool(false);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
