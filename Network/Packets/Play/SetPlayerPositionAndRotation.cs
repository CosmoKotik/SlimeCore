using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SetPlayerPositionAndRotation : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public double X { get; set; }
        public double FeetY { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public SetPlayerPositionAndRotation(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_PLAYER_POSITION_AND_ROTATION);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            X = bm.GetDouble();
            FeetY = bm.GetDouble();
            Z = bm.GetDouble();
            
            Yaw = bm.GetFloat();
            Pitch = bm.GetFloat();
            
            OnGround = bm.GetBool();
            return this;
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddDouble(0);
            bm.AddDouble(0);
            bm.AddDouble(0);
            
            bm.AddFloat(0);
            bm.AddFloat(0);

            bm.AddBool(false);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
