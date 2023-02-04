using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class SpawnPlayer
    {
        public Player Player { get; set; }
        private int _packetID = 0x05;
        private ClientHandler _handler;
        public SpawnPlayer(ClientHandler handler) 
        { 
            _handler = handler;
        }

        public void Write()
        {
            Vector3 spawn = new Vector3(2, 1, 0);
            //var data = (((long)spawn.X & 0x3FFFFFF) << 38) | (((long)spawn.Y & 0xFFF) << 26) | ((long)spawn.Z & 0x3FFFFFF);

            _handler.WriteVarInt(Player.EntityID);
            _handler.WriteUUID(new Guid(Player.Uuid));
            //_handler.WriteString(player.Uuid);
            _handler.WriteDouble(Player.Position.X * 32);
            _handler.WriteDouble(Player.Position.Y * 32);
            _handler.WriteDouble(Player.Position.Z * 32);
            _handler.WriteByte((byte)Player.Yaw);
            _handler.WriteByte((byte)Player.Pitch);

            //_handler.WriteShort(0);
            _handler.WriteByte(0xff);

            //_handler.WriteLong(data);
            _handler.Flush(_packetID);
        }
    }
}
