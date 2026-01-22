using fNbt;
using SlimeCore.Core.Chunks;
using SlimeCore.Core.Chunks.Loader;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Structs;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class WorldManager
    {
        public static LevelType LevelType { get; private set; }

        public static int WorldSizeX { get; set; }
        public static int WorldSizeZ { get; set; }

        internal static Chunk[] Chunks;
        internal static object ChunkLock = new object();

        private static int _chunk_size_x = 16;
        private static int _chunk_size_y = 16;
        private static int _chunk_size_z = 16;

        public WorldManager(int size_x, int size_z) 
        {
            Logger.Warn("Initializing world", true);

            WorldSizeX = size_x;
            WorldSizeZ = size_z;

            this.InitiateChunks();
        }

        private void InitiateChunks()
        {
            int total_size = WorldSizeX * WorldSizeZ;

            Chunks = new Chunk[total_size];

            for (int z = 0; z < WorldSizeZ; z++)
            {
                for (int x = 0; x < WorldSizeX; x++)
                {
                    int z_offset = z * WorldSizeZ;
                    int index = x + z_offset;
                    Chunks[index] = new Chunk(x, z);
                }
            }
        }

        public static void SetBlock(Block block)
        {
            Position chunk_pos = block.GetChunkPosition();
            int z_offset = (int)(chunk_pos.Z * WorldSizeZ);
            int chunk_index = z_offset + (int)chunk_pos.X;

            lock (ChunkLock) 
            {
                Chunks[chunk_index].SetBlock(block);
            }
        }

        public static void SetBlock(Position chunk_pos, Position local_block_chunk_pos, BlockType block_type)
        {
            int z_offset = (int)(chunk_pos.Z * WorldSizeZ);
            int chunk_index = z_offset + (int)chunk_pos.X;

            lock (ChunkLock)
            {
                Chunks[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            }
        }

        public static Chunk GetChunk(int x, int z)
        {
            int z_offset = z * WorldSizeZ;
            int chunk_index = z_offset + x;

            //Console.WriteLine($"x: {x} z: {z} index: {chunk_index}");

            lock (ChunkLock)
                return Chunks[chunk_index];
        }

        internal WorldManager GenerateFlatWorld(List<BlockType> layers, int y_offset = 0)
        {
            Logger.Log("Loading world...");
            Stopwatch stopwatch = Stopwatch.StartNew();

            LevelType = LevelType.FLAT;

            for (int chunk_z = 0; chunk_z < WorldSizeZ; chunk_z++)
            {
                for (int chunk_x = 0; chunk_x < WorldSizeX; chunk_x++)
                {

                    for (int y = 0; y < layers.Count; y++)
                    {
                        BlockType block_type = layers[y];
                        for (int x = 0; x < _chunk_size_x; x++)
                        {
                            for (int z = 0; z < _chunk_size_x; z++)
                            {
                                Position block_pos = new Position(x + (chunk_x * _chunk_size_x), y + y_offset, z + (chunk_z * _chunk_size_z));
                                Block block = new Block()
                                    .SetBlockType(block_type)
                                    .SetPosition(block_pos);

                                SetBlock(block);
                            }
                        }
                    }

                }
            }

            stopwatch.Stop();
            Logger.Log($"World loaded for {stopwatch.Elapsed.TotalMilliseconds}ms");

            return this;
        }

        internal WorldManager LoadWorldFromFile(string path)
        {
            Logger.Log("Loading world...");
            Stopwatch stopwatch = Stopwatch.StartNew();

            LevelType = LevelType.DEFAULT;

            var region = new RegionFile(path);

            for (int chunk_z = 0; chunk_z < 32; chunk_z++)
            {
                for (int chunk_x = 0; chunk_x < 32; chunk_x++)
                {
                    byte[] chunkData = region.ReadChunk(chunk_x, chunk_z);

                    if (chunkData != null)
                    {
                        var chunkNbt = WorldLoader.LoadChunkNBT(chunkData);
                        var blocks = WorldLoader.ParseChunk(chunkNbt);

                        for (int y = 0; y < blocks.Length / 256; y++)
                        {
                            int y_section = y / 16;
                            int block_y = y - (y_section * 16);
                            //Console.WriteLine(block_y);
                            for (int z = 0; z < 16; z++)
                            {
                                for (int x = 0; x < 16; x++)
                                {
                                    int y_offset = y * 256;
                                    int z_offset = z * 16;
                                    int block_index = y_offset + z_offset + x;

                                    BlockType block_type = blocks[block_index];
                                    /*Position block_pos = new Position(x + (chunk_x * _chunk_size_x), y, z + (chunk_z * _chunk_size_z));
                                    Block block = new Block()
                                        .SetBlockType(block_type)
                                        .SetPosition(block_pos);

                                    SetBlock(block);*/

                                    Position chunk_pos = new Position(chunk_x, y_section, chunk_z);
                                    Position local_block_chunk_pos = new Position(x, block_y, z);
                                    SetBlock(chunk_pos, local_block_chunk_pos, block_type);
                                }
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            Logger.Log($"World loaded for {stopwatch.Elapsed.TotalMilliseconds}ms");

            GC.Collect();

            return this;
        }
    }
}
