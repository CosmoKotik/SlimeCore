using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class PlayerCommand : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public PlayerCommand(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.PLAYER_COMMAND);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            int eid = bm.ReadVarInt();
            int actionId = bm.ReadVarInt();
            int jumpBoost = bm.ReadVarInt();

            switch (actionId)
            {
                case 0:
                    ClientHandler.ServerManager.Players.Find(x => x.EntityID.Equals(eid)).IsCrouching = true;
                    break;
                case 1:
                    ClientHandler.ServerManager.Players.Find(x => x.EntityID.Equals(eid)).IsCrouching = false;
                    break;
            }

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
