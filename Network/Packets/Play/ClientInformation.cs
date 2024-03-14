using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class ClientInformation : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColored { get; set; }
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public ClientInformation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.CLIENT_INFORMATION);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            Locale = bm.GetString();
            ViewDistance = bm.GetByte();
            ChatMode = bm.ReadVarInt();
            ChatColored = bm.GetBool();
            DisplayedSkinParts = bm.GetByte();
            MainHand = bm.ReadVarInt();
            EnableTextFiltering = bm.GetBool();
            AllowServerListings = bm.GetBool();

            return this;
        }

        public async Task Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
