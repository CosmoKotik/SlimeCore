using SlimeCore.Core.Chunks.Palettes;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SlimeCore.Core.Chunks
{
    public class ChunkSection
    {
        public byte BitsPerBlock { get; set; }  //fucking shit
        //public IPalette? Palette { get; set; }  //EVEN WORSE
        public int DataLength { get; set; }
        //public List<long> Data { get; set; } = new List<long>();
        public long[] Data { get; set; }
        public byte[] BlockLigth { get; set; }    //Half byte per block
        public byte[] SkyLight { get; set; }      //Optional shit

        private int _xSize = 16;
        private int _ySize = 16;
        private int _zSize = 16;

        //private Block[] _blocks;
        private BlockType[] _blocks;

        private Position _chunkPosition;

        public ChunkSection() { }
        public ChunkSection(int chunk_x, int chunk_z, int y)
        {
            int chunkSize = _xSize * _ySize * _zSize;
            //_blocks = new Block[chunkSize];
            _blocks = new BlockType[chunkSize];

            _chunkPosition = new Position(chunk_x, y, chunk_z);

            this.BitsPerBlock = 13;
            //this.Palette = new DirectPalette();

            InitializeBlocks();
        }

        private void InitializeBlocks(BlockType default_type = BlockType.Air)
        {
            int block_index = 0;
            for (int y = 0; y < _ySize; y++)
                for (int z = 0; z < _zSize; z++)
                    for (int x = 0; x < _xSize; x++)
                    {
                        /*Position pos = new Position(x, y, z);
                        Block block = new Block()
                            .SetBlockType(default_type)
                            .SetPosition(pos);*/

                        _blocks[block_index] = default_type;

                        block_index++;
                    }
        }

        public ChunkSection SetBlock(Block block)
        {
            Position b_pos = block.Position;
            Position chunk_offset_pos = new Position(
                _chunkPosition.X * _xSize,
                _chunkPosition.Y * _ySize,
                _chunkPosition.Z * _zSize);

            Position chunk_pos = b_pos - chunk_offset_pos;
            int index_y = (int)(chunk_pos.Y * (_xSize * _zSize));
            int index_z = (int)(chunk_pos.Z * _zSize);
            int index_x = (int)chunk_pos.X;

            int block_index = index_y + index_z + index_x;

            _blocks[block_index] = block.BlockType;

            return this;
        }

        /*public Block GetBlockFromLocalChunkPosition(Position local_position)
        {
            int index_y = (int)(local_position.Y * (_xSize * _zSize));
            int index_z = (int)(local_position.Z * _zSize);
            int index_x = (int)local_position.X;

            int block_index = index_y + index_z + index_x;

            return _blocks[block_index];
        }*/

        public ChunkSection GenerateChunkSection()
        {
            int dataLength = ((_xSize * _ySize * _zSize) * this.BitsPerBlock) / 64;
            long[] data = new long[dataLength];
            uint individualValueMask = (uint)((1 << this.BitsPerBlock) - 1);

            for (int y = 0; y < _ySize; y++)
            {
                for (int z = 0; z < _zSize; z++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        int blockNumber = (((y * _ySize) + z) * _xSize) + x;
                        int startLong = (blockNumber * this.BitsPerBlock) / 64;
                        int startOffset = (blockNumber * this.BitsPerBlock) % 64;
                        int endLong = ((blockNumber + 1) * this.BitsPerBlock - 1) / 64;

                        int index_y = y * (_xSize * _zSize);
                        int index_z = z * _zSize;
                        int index_x = x;

                        int block_index = index_y + index_z + index_x;

                        /*byte metadata = 0;
                        uint id = (uint)block_id;

                        long value = id << 4 | metadata;*/

                        //Block block = GetBlockFromLocalChunkPosition(new Position(x, y, z));
                        //long value = block.GetIDWithMeta();
                        long value = (long)_blocks[block_index];
                        value &= individualValueMask;

                        data[startLong] |= (value << startOffset);

                        if (startLong != endLong)
                            data[endLong] = (value >> (64 - startOffset));
                    }
                }
            }

            this.DataLength = dataLength;
            this.Data = data;

            //fuck next section of code, unimplemented piece of shit code
            byte blockLight = 0;
            //List<byte> blockLightArray = new List<byte>();
            byte[] blockLightArray = new byte[4096];

            byte skyLight = 1;
            //List<byte> skyLightArray = new List<byte>();
            byte[] skyLightArray = new byte[4096];

            for (int y = 0; y < _ySize; y++)
                for (int z = 0; z < _zSize; z++)
                    for (int x = 0; x < _xSize; x += 2)
                    {
                        byte value = (byte)(blockLight | (blockLight << 4));

                        int y_offset = y * _zSize * _xSize;
                        int z_offset = z * _zSize;
                        int index = x + y_offset + z_offset;

                        //blockLightArray.Add(value);
                        blockLightArray[index] = value;
                    }

            for (int y = 0; y < _ySize; y++)
                for (int z = 0; z < _zSize; z++)
                    for (int x = 0; x < _xSize; x += 2)
                    {
                        byte value = (byte)(skyLight | (skyLight << 4));

                        int y_offset = y * _zSize * _xSize;
                        int z_offset = z * _zSize;
                        int index = x + y_offset + z_offset;

                        //skyLightArray.Add(value);
                        skyLightArray[index] = value;
                    }

            this.BlockLigth = blockLightArray;
            this.SkyLight = skyLightArray;

            return this;
        }

        [Obsolete("Method is deprecated, please use GenerateChunkSection() instead.")]
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

            /*IPalette palette;

            if (bitsPerBlock < 9)
            { 
                palette = new IndirectPalette();
                palette.PaletteLength = xSize * ySize * zSize;
                for (int i = 0; i < palette.PaletteLength; i++)
                    ((IndirectPalette)palette).Palette.Add(0);
            }
            else
                palette = new DirectPalette();

            section.Palette = palette;*/
            
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
            section.Data = data;

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
            ChunkSection generated_section = section.GenerateChunkSection();

            BufferManager bm = new BufferManager();
            bm.WriteByte(generated_section.BitsPerBlock);
            //bm.WriteBytes(generated_section.Palette.GetBytes(), false);
            bm.WriteByte(0);
            bm.WriteVarInt(generated_section.DataLength);
            
            for (int i = 0; i < generated_section.DataLength; i++)
                bm.WriteLong(generated_section.Data[i]);

            bm.WriteBytes(generated_section.BlockLigth, false);
            bm.WriteBytes(generated_section.SkyLight, false);

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
