using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class ChatMessage : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public string Message { get; set; }
        public long Timestamp { get; set; }
        public long Salt { get; set; }
        public bool HasSignature { get; set; }
        public byte[] Signature { get; set; }
        public int MessageCount { get; set; }
        public byte[] Bitset { get; set; }

        public ChatMessage(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.CHAT_MESSAGE);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            //Console.WriteLine("message: {0}", BitConverter.ToString(bm.GetBytes()).Replace("-", " "));
            string message = bm.GetString();
            long timestamp = bm.GetLong();
            long salt = bm.GetLong();

            bool hasSignature = bm.GetBool();

            byte[] signature = new byte[256];
            if (hasSignature)
                for (int i = 0; i < signature.Length; i++)
                    signature[i] = bm.GetByte();

            int messageCount = bm.ReadVarInt();

            /*byte[] bitset = new byte[20];

            for (int i = 0; i < bitset.Length; i++)
                bitset[i] = bm.GetByte();*/

            this.Message = message;
            this.Timestamp = timestamp;
            this.Salt = salt;
            this.HasSignature = hasSignature;
            this.Signature = signature;
            this.MessageCount = messageCount;
            //this.Bitset = bitset;

            return this;
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
