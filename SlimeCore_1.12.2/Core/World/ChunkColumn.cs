using fNbt;
using SlimeCore.Core.Entity;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SlimeCore.Core.World
{
    public class ChunkColumn
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public ushort[] Blocks = new ushort[16 * 16 * 256];
        public int[] BiomeColor = ArrayOf<int>.Create(256, 1);
        public byte[] BiomeId = ArrayOf<byte>.Create(256, 1);

        public bool IsDirty = false;

        public ushort[] Metadata = new ushort[16 * 16 * 256];
        
        public NibbleArray Blocklight = new NibbleArray(16 * 16 * 256);
        public NibbleArray Skylight = new NibbleArray(16 * 16 * 256);

        public Chunk Chnk { get; set; }

        public ChunkColumn()
        {
            for (var i = 0; i < Skylight.Length; i++)
				Skylight[i] = 0xff;
			for (var i = 0; i < BiomeColor.Length; i++)
				BiomeColor[i] = 8761930;
			for (var i = 0; i < Metadata.Length; i++)
				Metadata[i] = 0;

            //Chnk = new FlatLand().GetChunck(ChunkX, ChunkZ);

            /*for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    for (int z = 0; z < 10; z++)
                        SetBlock(new Vector3(x, y, z));*/
        }

        public byte[] GetBytes()
        {
            List<byte> buffer = new List<byte>();
            List<byte> chunkBuffer = new List<byte>();
            BufferManager bm = new BufferManager();

            /*bm.WriteInt(ChunkX, ref buffer);
            bm.WriteInt(ChunkZ, ref buffer);
            bm.WriteBool(true, ref buffer);
            bm.WriteUShort(0, ref buffer);
            bm.WriteVarInt(0, ref buffer);
            */

            //SetBlock(0, 0, 0, 2, 0);



            bm.WriteInt(ChunkX, ref buffer);
            bm.WriteInt(ChunkZ, ref buffer);
            bm.WriteBool(true, ref buffer);
            bm.WriteVarInt(4, ref buffer);

            ChunkSection ip = new ChunkSection();
            byte[] chunkBlocks = ip.Get(4096, false);
            byte[] chunkBiome = ip.Get(16, true);

            //bm.WriteVarInt((chunkBlocks.Length), ref buffer);

            bm.WriteShort(64, ref buffer);
            //bm.Write(chunkBlocks, ref buffer);
            bm.Write(chunkBiome, ref buffer);

            bm.WriteVarInt(0, ref buffer);




            /*bm.WriteInt(ChunkX, ref buffer);
            bm.WriteInt(ChunkZ, ref buffer);
            bm.WriteBool(true, ref buffer);
            bm.WriteUShort(0xffff, ref buffer);
            bm.WriteVarInt(0, ref buffer);

            var rawData = Chnk.RawData.Write();

            bm.WriteShort((short)(rawData.Length + 255), ref buffer);

            for (int i = 0; i < rawData.Length; i++)
                bm.WriteByte(rawData[i], ref buffer);

            for (int i = 0; i < 256; i++)
                bm.WriteVarInt(1, ref buffer);

            bm.WriteVarInt(0, ref buffer);*/

            //bm.WriteVarInt(0, ref buffer);

            /*bm.WriteInt(ChunkX, ref buffer);
            bm.WriteInt(ChunkZ, ref buffer);
            bm.WriteBool(true, ref buffer);
            bm.WriteUShort(0xFFFF, ref buffer); // bitmap
            bm.WriteVarInt((Blocks.Length * 2) + Skylight.Data.Length + Blocklight.Data.Length + BiomeId.Length, ref buffer);

            for (var i = 0; i < Blocks.Length; i++)
            {
                bm.WriteUShort((ushort)((Blocks[i] << 4) | Metadata[i]), ref buffer);
            }

            bm.Write(Blocklight.Data, ref buffer);
            bm.Write(Skylight.Data, ref buffer);

            bm.Write(BiomeId, ref buffer);*/

            /*List<byte> buffer = new List<byte>();
            BufferManager bufferManager = new BufferManager();

            bufferManager.WriteInt32(ChunkX, ref buffer);
            bufferManager.WriteInt32(ChunkZ, ref buffer);
            bufferManager.WriteBool(true, ref buffer);
            bufferManager.WriteUInt16(0xffff, ref buffer);
            bufferManager.WriteVarInt((Blocks.Length * 2 * Blocklight.Data.Length * 2) + BiomeId.Length, ref buffer);

            for (var i = 0; i < Blocks.Length; i++)
            {
                bufferManager.WriteUShort((ushort)((Blocks[i] << 4) | Metadata[i]), ref buffer);
                //bufferManager.WriteUShort((ushort)((Blocks[i] << 4) | Metadata[i]), ref buffer);
            }

            bufferManager.Write(Blocklight.Data, ref buffer);
            bufferManager.Write(Skylight.Data, ref buffer);

            bufferManager.Write(BiomeId, ref buffer);
            
            //bufferManager.WriteVarInt(Blocks.Length, ref buffer);

            //bufferManager.WriteByte(0, ref buffer);*/

            return buffer.ToArray();
        }

        public void SendChunkData()
        {
            
        }
    }
}
