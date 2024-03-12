using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class ChatCommand : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public string Command { get; set; }
        public long Timestamp { get; set; }
        public long Salt { get; set; }
        public int SignatureArrayLength { get; set; }
        public string[] SignatureArgumentName { get; set; }
        public List<byte[]> Signatures { get; set; }
        public int MessageCount { get; set; }
        public byte[] Bitset { get; set; }

        public ChatCommand(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.CHAT_COMMAND);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            string command = bm.GetString();
            long timestamp = bm.GetLong();
            long salt = bm.GetLong();
            int signatureArrayLength = bm.ReadVarInt();

            string[] signatureArgumentName = new string[signatureArrayLength];
            List<byte[]> signatures = new List<byte[]>();

            int messageCount = bm.ReadVarInt();

            /*byte[] bitset = new byte[20];

            for (int i = 0; i < bitset.Length; i++)
                bitset[i] = bm.GetByte();*/

            this.Command = command;
            this.Timestamp = timestamp;
            this.Salt = salt;
            this.SignatureArrayLength = signatureArrayLength;
            this.Signatures = signatures;
            this.MessageCount = messageCount;
            this.SignatureArgumentName = signatureArgumentName;
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
