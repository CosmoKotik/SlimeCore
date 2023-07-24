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
            ChunkColumn c = new ChunkColumn() { ChunkX = 0, ChunkZ = 0 };
            _handler.Write(c.GetBytes());
            _handler.Flush(_packetID);

            ChunkColumn c1 = new ChunkColumn() { ChunkX = 0, ChunkZ = 1 };
            _handler.Write(c1.GetBytes());
            _handler.Flush(_packetID);
        }
    }
}
