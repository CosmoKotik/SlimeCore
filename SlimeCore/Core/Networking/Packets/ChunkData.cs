using SlimeCore.Core.Entity;
using SlimeCore.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class ChunkData
    {

        private int _packetID = 0x20;
        private ClientHandler _handler;
        public ChunkData(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            for (int x = 0; x < 1; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    ChunkColumn c = new ChunkColumn() { ChunkX = x, ChunkZ = z };
                    _handler.Write(c.GetBytes());
                    _handler.Flush(_packetID);
                }
            }
        }
    }
}
