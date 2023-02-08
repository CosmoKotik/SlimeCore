using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class PlayerPositionAndLook
    {
        private int _packetID = 0x2F;
        private ClientHandler _handler;
        public PlayerPositionAndLook(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            Vector3 spawn = new Vector3(2, 1, 40);

            Player player = _handler.CurrentPlayer;

            _handler.WriteDouble(spawn.X);
            _handler.WriteDouble(spawn.Y);
            _handler.WriteDouble(spawn.Z);
            _handler.WriteFloat(14);
            _handler.WriteFloat(53);
            _handler.WriteByte(111);
            _handler.WriteVarInt(1);

            //_handler.WriteLong(data);
            _handler.Flush(_packetID);
        }

        public void Read()
        {
            Vector3 pos = _handler.ReadPosition();
            float Yaw = _handler.ReadFloat();
            float Pitch = _handler.ReadFloat();
            bool OnGround = _handler.ReadBool();

            Console.WriteLine(pos);

            _handler.CurrentPlayer.PositionAndLookChanged(pos, (int)Yaw, (int)Pitch, OnGround);
        }
    }
}
