using SlimeCore.Core.Networking;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.World
{
    internal class Chunk
    {
        public int ChunkX { get; set; }
        public int ChunkY { get; set; }

        public byte[] BlockLightData = new byte[16 * 16];
        public byte[] SkylightData = new byte[16 * 16 * 256];
        public byte[] BiomeId = new byte[64];

        public byte[] GetWorldBytes()
        {
            ushort[] Blocks = new ushort[16 * 16];

            for (int i = 0; i < 256; i++)
            {
                Blocks[i] = 2;
                BlockLightData[i] = 2;
            }
            List<byte> buffer = new List<byte>();
            BufferManager bufferManager = new BufferManager();

            bufferManager.WriteInt(ChunkX, ref buffer);
            bufferManager.WriteInt(ChunkY, ref buffer);
            bufferManager.WriteBool(true, ref buffer);
            bufferManager.WriteUShort(0, ref buffer);
            bufferManager.WriteVarInt(0, ref buffer);
            /*bufferManager.WriteVarInt(0xffff, ref buffer);

            bufferManager.WriteVarInt((Blocks.Length * 2), ref buffer); //Size

            //bufferManager.WriteShort((short)((BiomeId.Length * 2) + Blocks.Length + 2), ref buffer);
            bufferManager.WriteShort((short)Blocks.Length, ref buffer);

            for (var i = 0; i < Blocks.Length; i++)
            {
                //bufferManager.WriteByte(Convert.ToByte(4096), ref buffer);
                //bufferManager.WriteVarInt(2, ref buffer);
                bufferManager.WriteUShort(2 << 4 | 0, ref buffer);
                bufferManager.WriteUShort(2 >> 4, ref buffer);
            }
            //bufferManager.Write(BlockLightData, ref buffer);
            //bufferManager.Write(BlockLightData, ref buffer);
            bufferManager.Write(BiomeId, ref buffer);
            //bufferManager.WriteVarInt((BiomeId.Length * 2) + Blocks.Length, ref buffer);

            //bufferManager.Write(BlockLightData, ref buffer);
            //bufferManager.Write(SkylightData, ref buffer);*/


            return buffer.ToArray();
        }
    }
}
