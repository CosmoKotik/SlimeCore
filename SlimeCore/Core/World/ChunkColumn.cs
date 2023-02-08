using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.World
{
    internal class ChunkColumn
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public ushort[] Blocks = new ushort[16 * 16 * 16];
        public NibbleArray Blocklight = new NibbleArray(16 * 16 * 16);
        public int[] BiomeColor = ArrayOf<int>.Create(64, 1);
        public byte[] BiomeId = ArrayOf<byte>.Create(64, 1);
        public NibbleArray Skylight = new NibbleArray(16 * 16 * 256);
        public ushort[] Metadata = new ushort[16 * 16 * 16];

        public ChunkColumn()
        {
            for (var i = 0; i < Skylight.Length; i++)
                Skylight[i] = 0xff;
            for (var i = 0; i < BiomeColor.Length; i++)
                BiomeColor[i] = 8761930;
            for (var i = 0; i < Metadata.Length; i++)
                Metadata[i] = 0;
        }

        public byte[] GetBytes()
        {
            List<byte> buffer = new List<byte>();
            BufferManager bufferManager = new BufferManager();

            bufferManager.WriteInt(ChunkX, ref buffer);
            bufferManager.WriteInt(ChunkZ, ref buffer);
            bufferManager.WriteBool(true, ref buffer);
            bufferManager.WriteVarInt(0xffff, ref buffer);
            bufferManager.WriteVarInt((Blocks.Length * 2) + Skylight.Data.Length + Blocklight.Data.Length + BiomeId.Length, ref buffer);

            for (var i = 0; i < Blocks.Length; i++)
            {
                bufferManager.WriteUShort((ushort)((Blocks[i] << 4) | Metadata[i]), ref buffer);
            }

            bufferManager.Write(Blocklight.Data, ref buffer);
            bufferManager.Write(Skylight.Data, ref buffer);

            bufferManager.Write(BiomeId, ref buffer);
            
            bufferManager.WriteVarInt(0, ref buffer);

            return buffer.ToArray();
        }
    }
}
