using SlimeCore.Entity;
using SlimeCore.Enums;
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

        public PlayerInfoUpdate(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.PLAYER_INFO_UPDATE);
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

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);
            bm.AddBytes(_bufferManager.GetBytes(), false);
            
            await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public PlayerInfoUpdate AddPlayer(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x01);

            _bufferManager.AddVarInt(1);

            _bufferManager.AddUUID(player.UUID);
            _bufferManager.AddString(player.Username);
            _bufferManager.AddVarInt(0);

            return this;
        }

        public PlayerInfoUpdate InitializeChat(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x02);
            _bufferManager.AddBool(false);

            return this;
        }

        public PlayerInfoUpdate UpdateGameMode(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x04);
            _bufferManager.AddVarInt(player.Gamemode);

            return this;
        }

        public PlayerInfoUpdate UpdateListed(Player player)
        {
            _bufferManager = new BufferManager();

            _bufferManager.AddByte(0x04);
            _bufferManager.AddVarInt(player.Gamemode);

            return this;
        }
    }
}
