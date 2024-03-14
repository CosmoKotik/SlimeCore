using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Status
{
    internal class PingLegacy : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public PingLegacy(ClientHandler clientHandler) 
        {
            this.ClientHandler = clientHandler;
            this.PacketID = 0xff;
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
            //bm.InsertBytes(Converters.StringToByteArray("00 a7 00 31 00 00"));
            bm.AddByte(0x00);
            bm.AddByte(0x23);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('§')[0]);
            bm.AddByte(0x00);
            bm.AddByte(31);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('7')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('5')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('9')[0]);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('1')[0]);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('1')[0]);
            bm.AddByte(BitConverter.GetBytes('9')[0]);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('P')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('O')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('R')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('N')[0]);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('0')[0]);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('6')[0]);
            bm.AddByte(0x00);
            bm.AddByte(BitConverter.GetBytes('9')[0]);

            QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
