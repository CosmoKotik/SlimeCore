using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks
{
    public class Chunk
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool GroundUpContinuous { get; set; }
        public varint PrimaryBitMask { get; set; }
        public varint DataSize { get; set; }
        public byte[] Data { get; set; }
        public varint NumberOfBlockEntities { get; set; }
        public byte[] BlockEntities { get; set; }

        private ChunkSection[] _chunkSections;
        private int _height = 16;

        public Chunk()
        { 
            _chunkSections = new ChunkSection[16];
        }
        public Chunk(int chunk_x, int chunk_z)
        {
            this.ChunkX = chunk_x;
            this.ChunkZ = chunk_z;

            _chunkSections = new ChunkSection[16];

            InitializeChunkSections();
        }

        private void InitializeChunkSections()
        { 
            for (int i = 0; i < _height; i++)
                _chunkSections[i] = new ChunkSection(this.ChunkX, this.ChunkZ, i);
        }

        public Chunk SetBlock(Block block)
        {
            Position chunk_pos = block.GetChunkPosition();
            int y_section = (int)chunk_pos.Y;

            //Console.WriteLine(chunk_pos.ToString());

            _chunkSections[y_section].SetBlock(block);
            return this;
        }

        public Chunk SetBlock(Position chunk_pos, Position local_block_chunk_pos, BlockType block_type)
        {
            int y_section = (int)chunk_pos.Y;

            _chunkSections[y_section].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            return this;
        }
        public Chunk SetBlock(Position chunk_pos, Position local_block_chunk_pos, ushort block_type)
        {
            int y_section = (int)chunk_pos.Y;

            _chunkSections[y_section].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            return this;
        }

        public Block GetBlock(Position chunk_pos, Position local_block_chunk_pos)
        {
            int y_section = (int)chunk_pos.Y;

            return _chunkSections[y_section].GetBlock(local_block_chunk_pos);
        }

        public Chunk GenerateChunk(bool continuous = true)
        {
            this.GroundUpContinuous = continuous;

            int mask = 0;
            using (BufferManager bm = new BufferManager())
            { 
                for (int y = 0; y < _height; y++)
                {
                    mask |= (1 << y);
                    ChunkSection section = _chunkSections[y];
                    bm.WriteBytes(ChunkSection.GetBytes(section), false);
                }

                if (continuous)
                    for (int z = 0; z < 16; z++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            bm.WriteByte(127);  //Currently biomes are not supported
                        }
                    }

                byte[] data = bm.GetBytes();

                PrimaryBitMask = mask;
                DataSize = data.Length;
                Data = data;

                NumberOfBlockEntities = 0;  //Currently block entities not supported

                return this;
            }

        }

        [Obsolete("Method is deprecated, please use GenerateChunk() instead.")]
        public void Build(int xPos, int zPos, bool continuous = true, int block_id = 1)
        { 
            this.ChunkX = xPos;
            this.ChunkZ = zPos;
            this.GroundUpContinuous = continuous;

            int mask = 0;
            BufferManager bm = new BufferManager();

            for (int y = 0; y < 8; y++)
            {
                mask |= (1 << y);
                ChunkSection section = ChunkSection.GenerateFill(block_id, false);
                bm.WriteBytes(ChunkSection.GetBytes(section), false);
            }

            if (continuous)
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        bm.WriteByte(127);  //Currently biomes are not supported
                    }
                }

            byte[] data = bm.GetBytes();

            PrimaryBitMask = mask;
            DataSize = data.Length;
            Data = data;

            NumberOfBlockEntities = 0;  //Currently block entities not supported
        }

        public byte[] GetBytes()
        {
            this.GenerateChunk();

            using (BufferManager bm = new BufferManager())
            { 
                bm.WriteInt(this.ChunkX);
                bm.WriteInt(this.ChunkZ);
                bm.WriteBool(this.GroundUpContinuous);
                bm.WriteVarInt(this.PrimaryBitMask);
                bm.WriteVarInt(this.DataSize);
                bm.WriteBytes(this.Data, false);
                bm.WriteVarInt(this.NumberOfBlockEntities);

                return bm.GetBytes();
            }
        }
    }
}
