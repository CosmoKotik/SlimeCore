using SlimeCore.Core.Chunks;
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

        public static Chunk GetChunk(int x, int z)
        {
            int z_offset = (int)(z * WorldSizeZ);
            int chunk_index = z_offset + (int)x;

            //Console.WriteLine($"x: {x} z: {z} index: {chunk_index}");

            lock (ChunkLock)
                return Chunks[chunk_index];
        }

        internal WorldManager GenerateFlatWorld(List<BlockType> layers)
        {
            Logger.Log("Loading world...");
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int y = 0; y < layers.Count; y++)
            {
                BlockType block_type = layers[y];
                for (int z = 0; z < WorldSizeZ; z++)
                {
                    for (int x = 0; x < WorldSizeX; x++)
                    {
                        Position block_pos = new Position(x, y, z);
                        Block block = new Block()
                            .SetBlockType(block_type)
                            .SetPosition(block_pos);

                        SetBlock(block);
                    }
                }
            }

            stopwatch.Stop();
            Logger.Log($"World loaded for {stopwatch.Elapsed.TotalMilliseconds}ms");

            return this;
        }
    }
}
