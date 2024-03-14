using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
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

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            throw new NotImplementedException();
        }

        public async Task Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddDouble(0);
            bm.AddDouble(0);
            bm.AddDouble(0);
            bm.AddFloat(0);
            bm.AddFloat(0);

            //Set relative y axis
            bm.AddByte(0);

            bm.AddVarInt(1);
            //bm.AddBool(false);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public async Task Write(Position position)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddDouble(position.PositionX);   //Set position x
            bm.AddDouble(position.PositionY);   //Set position y
            bm.AddDouble(position.PositionZ);   //Set position z
            bm.AddFloat(position.Yaw);          //Set yaw
            bm.AddFloat(position.Pitch);        //Set pitch

            //Set relative x axis
            bm.AddByte(0);

            //Set teleportation id (for now its 1)
            bm.AddVarInt(1);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
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
