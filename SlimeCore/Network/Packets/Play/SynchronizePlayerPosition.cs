using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SynchronizePlayerPosition : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SynchronizePlayerPosition(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SYNCHRONIZE_PLAYER_POSITION);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            throw new NotImplementedException();
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            /*bm.AddDouble(new Random().Next(0, 20));
            bm.AddDouble(new Random().Next(0, 20));
            bm.AddDouble(new Random().Next(0, 20));*/
            bm.AddDouble(0);
            bm.AddDouble(0);
            bm.AddDouble(0);
            bm.AddFloat(0);
            bm.AddFloat(0);

            //Set relative y axis
            bm.AddByte(0x02);

            bm.AddVarInt(0);
            //bm.AddBool(false);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }


    /*
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SynchronizePlayerPosition(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SYNCHRONIZE_PLAYER_POSITION);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            throw new NotImplementedException();
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);



            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    */
}
