using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class PlayerInfoUpdate : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        private BufferManager _bufferManager = new BufferManager();

        private bool _broadcast = false;
        private bool _includeSelf = false;

        public PlayerInfoUpdate(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.PLAYER_INFO_UPDATE);
        }

        public PlayerInfoUpdate Broadcast(bool includeSelf)
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

        public async Task Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            bm.AddBytes(_bufferManager.GetBytes(), false);

            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public PlayerInfoUpdate AddPlayer(Player player)
        {
            _bufferManager = new BufferManager();

            //Mask
            _bufferManager.AddByte(0x01);

            //Number of players
            _bufferManager.AddVarInt(1);

            //UUID
            _bufferManager.AddUUID(player.UUID);
            
            //Player
            _bufferManager.AddString(player.Username);
            _bufferManager.AddVarInt(0);    //Number of properties, fuck that

            return this;
        }

        public PlayerInfoUpdate InitializeChat(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x02);
            
            //Number of players
            _bufferManager.AddVarInt(1);

            //UUID
            _bufferManager.AddUUID(player.UUID);

            _bufferManager.AddBool(false);

            return this;
        }

        public PlayerInfoUpdate UpdateGameMode(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x04);

            //Number of players
            _bufferManager.AddVarInt(1);

            //UUID
            _bufferManager.AddUUID(player.UUID);

            _bufferManager.AddVarInt(player.Gamemode);

            return this;
        }

        public PlayerInfoUpdate UpdateListed(Player player, bool isListed)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x08);

            //Number of players
            _bufferManager.AddVarInt(1);

            //UUID
            _bufferManager.AddUUID(player.UUID);

            _bufferManager.AddBool(isListed);

            return this;
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
