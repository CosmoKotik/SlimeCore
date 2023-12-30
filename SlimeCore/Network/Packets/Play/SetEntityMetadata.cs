using SlimeCore.Entity;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SetEntityMetadata : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public SetEntityMetadata(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_ENTITY_METADATA);
        }

        public void Broadcast(bool includeSelf)
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

        public async void Write(Player player, MetadataType meta)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(player.EntityID);

            //bm.AddVarInt(1);

            switch (meta)
            {
                case MetadataType.IsCrouching:
                    //bm.AddByte((0 << 5 | 0 & 0x1F) & 0xFF);
                    byte flag = 0;
                    flag |= 0x05;
                    //flag |= 0x02;
                    //flag |= 0x40;
                    //flag |= 0xFF;
                    //bm.AddByte(player.IsCrouching ? (byte)1 : (byte)0);

                    bm.AddByte(6);  //Index
                    bm.AddByte(20);  //Type
                    bm.AddByte(player.IsCrouching ? (byte)5 : (byte)0);   //Value
                    bm.AddByte(0xFF);   //End
                    break;
            }

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
