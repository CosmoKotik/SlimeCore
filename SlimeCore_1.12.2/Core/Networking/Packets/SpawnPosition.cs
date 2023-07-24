using SlimeCore.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class SpawnPosition
    {
        private int _packetID = 0x46;
        private ClientHandler _handler;
        public SpawnPosition(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            Vector3 spawn = new Vector3(0, 0, 0);
            var data = (((long)spawn.X & 0x3FFFFFF) << 38) | (((long)spawn.Y & 0xFFF) << 26) | ((long)spawn.Z & 0x3FFFFFF);

            Player player = _handler.CurrentPlayer;

            _handler.WriteLong(data);
            _handler.Flush(_packetID);
        }
    }
}
