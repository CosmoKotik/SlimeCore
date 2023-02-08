using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class PlayerPosition
    {
        public int BlockId;
        public System.Numerics.Vector3 Position;
        public int Metadata;

        private int _packetID = 0x0B;
        private ClientHandler _handler;
        public PlayerPosition(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Read()
        {
            /*float X = Convert.ToSingle(_handler.ReadDouble());
            float FeetY = Convert.ToSingle(_handler.ReadDouble());
            float Z = Convert.ToSingle(_handler.ReadDouble());*/
            Vector3 pos = _handler.ReadPosition();
            bool OnGround = _handler.ReadBool();

            Console.WriteLine(pos);

            _handler.CurrentPlayer.PositionChanged(pos, OnGround);
        }
    }
}
