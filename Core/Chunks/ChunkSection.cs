using SlimeCore.Core.Chunks.Palettes;
using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks
{
    public class ChunkSection
    {
        public byte BitsPerBlock { get; set; }  //fucking shit
        public IPalette? Palette { get; set; }  //EVEN WORSE
        public varint DataLength { get; set; }
        public List<long> Data { get; set; } = new List<long>();
        public byte[] BlockLigth { get; set; }    //Half byte per block
        public byte[] SkyLight { get; set; }      //Optional shit

        public static ChunkSection GenerateFill(int block_id, bool random_block_id)
        {
            int xSize = 16;
            int ySize = 16;
            int zSize = 16;

            int blockCount = xSize * ySize * zSize;

            ChunkSection section = new ChunkSection();
            //byte bitsPerBlock = GetBitsPerBlock(block_id);
            byte bitsPerBlock = 13;

            section.BitsPerBlock = bitsPerBlock;

            IPalette palette;

            if (bitsPerBlock < 9)
            { 
                palette = new IndirectPalette();
                palette.PaletteLength = xSize * ySize * zSize;
                for (int i = 0; i < palette.PaletteLength; i++)
                    ((IndirectPalette)palette).Palette.Add(0);
            }
            else
                palette = new DirectPalette();

            section.Palette = palette;
            
            int dataLength = ((xSize * ySize * zSize) * bitsPerBlock) / 64;
            long[] data = new long[dataLength];
            uint individualValueMask = (uint)((1 << bitsPerBlock) - 1);

            Random rnd = new Random();

            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        if (random_block_id)
                            block_id = rnd.Next(1, 200);

                        int blockNumber = (((y * ySize) + z) * xSize) + x;
                        int startLong = (blockNumber * bitsPerBlock) / 64;
                        int startOffset = (blockNumber * bitsPerBlock) % 64;
                        int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;

                        byte metadata = 0;
                        uint id = (uint)block_id;

                        long value = id << 4 | metadata;
                        value &= individualValueMask;

                        data[startLong] |= (value << startOffset);

                        if (startLong != endLong)
                            data[endLong] = (value >> (64 - startOffset));
                    }
                }
            }

            section.DataLength = dataLength;
            section.Data = data.ToList();

            byte blockLight = 0;
            List<byte> blockLightArray = new List<byte>();
            
            byte skyLight = 1;
            List<byte> skyLightArray = new List<byte>();

            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                    for (int x = 0; x < xSize; x += 2)
                    {
                        byte value = (byte)(blockLight | (blockLight << 4));
                        blockLightArray.Add(value);
                    }

            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                    for (int x = 0; x < xSize; x += 2)
                    {
                        byte value = (byte)(skyLight | (skyLight << 4));
                        skyLightArray.Add(value);
                    }

            section.BlockLigth = blockLightArray.ToArray();
            section.SkyLight = skyLightArray.ToArray();

            return section;
        }

        public static byte[] GetBytes(ChunkSection section)
        {
            BufferManager bm = new BufferManager();
            bm.WriteByte(section.BitsPerBlock);
            bm.WriteBytes(section.Palette.GetBytes(), false);
            bm.WriteVarInt(section.DataLength);
            
            for (int i = 0; i < section.DataLength; i++)
                bm.WriteLong(section.Data[i]);

            bm.WriteBytes(section.BlockLigth, false);
            bm.WriteBytes(section.SkyLight, false);

            return bm.GetBytes();
        }

        private static byte GetBitsPerBlock(int block_id)
        {
            int bits = (int)Math.Ceiling(Math.Log2(block_id));
            if (bits <= 4)
                bits = 4;
            return (byte)bits;
        }
    }
}
