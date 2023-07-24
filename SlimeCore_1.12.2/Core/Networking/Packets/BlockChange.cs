using SlimeCore.Core.Entity;
using SlimeCore.Core.Utils;
using System;
using System.Buffers.Binary;
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
        public Utils.Vector3 Position;
        public int Metadata;

        private int _packetID = 0x0B;
        private ClientHandler _handler;
        public BlockChange(ClientHandler handler)
        {
            _handler = handler;
        }

        public void Write()
        {
            long x = ((Position.x & 0x3FFFFFF) << 38);
            long y = ((Position.y & 0xFFF) << 26);
            long z = (Position.z & 0x3FFFFFF);

            //var data = (((int)Position.x & 0x3FFFFFF) << 38) | (((int)Position.y & 0xFFF) << 26) | (((int)Position.z & 0x3FFFFFF));

            //_handler.WriteLong(data);
            //_handler.WriteVarInt(BlockId << 4 | (Metadata & 15));

            if (x >= (2 ^ 25)) { x -= 2 ^ 26; }
            if (y >= (2 ^ 11)) { y -= 2 ^ 12; }
            if (z >= (2 ^ 25)) { z -= 2 ^ 26; }

            var data = new byte[64];

            //Console.WriteLine(((Position.x & 0x3FFFFFF) << 38) | ((Position.y & 0xFFF) << 26) | (Position.z & 0x3FFFFFF));

            //_handler.WriteLong(((Position.x & 0x3FFFFFF) << 38) | ((Position.y & 0xFFF) << 26) | (Position.z & 0x3FFFFFF));
            //_handler.WriteLong((long)(x | y | z));
            
            Console.WriteLine(EncodeBlockPosition(Position.x, Position.y, Position.z).ToString());

            _handler.WriteULong(EncodeBlockPosition(Position.x, Position.y, Position.z));

            //_handler.WriteVector3(new System.Numerics.Vector3(x, y, z));
            _handler.WriteVarInt(BlockId << 4 | (Metadata & 0xf));

            //_handler.WriteVector3(Position);

            //_handler.WriteLong(data);
            _handler.Flush(_packetID);
        }

        public static ulong EncodeBlockPosition(long x, long y, long z)
        {
            return (ulong)(((x & 0x3FFFFFF) << 38) | ((y & 0xFFF) << 26) | (z & 0x3FFFFFF));
        }
    }
}
