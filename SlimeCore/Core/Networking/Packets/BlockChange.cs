using SlimeCore.Core.Entity;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    internal class BlockChange
    {
        public int BlockId;
        public System.Numerics.Vector3 Position;
        public int Metadata;

        private int _packetID = 0x0B;
        private ClientHandler _handler;
        public BlockChange(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            var data = (((int)Position.X & 0x3FFFFFF) << 38) | (((int)Position.Y & 0xFFF) << 26) | (((int)Position.Z & 0x3FFFFFF));

            _handler.WriteLong(data);
            //_handler.WriteVector3(Position);
            _handler.WriteVarInt(BlockId << 4 | (Metadata & 15));

            //_handler.WriteLong(data);
            _handler.Flush(_packetID);
        }
    }
}
