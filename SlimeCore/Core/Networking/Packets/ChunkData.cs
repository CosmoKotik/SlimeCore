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
            Chunk c = new Chunk();
            _handler.Write(c.GetWorldBytes());

            //_handler.WriteLong(data);
            _handler.Flush(_packetID);
        }
    }
}
