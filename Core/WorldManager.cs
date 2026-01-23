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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class WorldManager
    {
        public static LevelType LevelType { get; private set; }

        public static int WorldSizeX { get; set; }
        public static int WorldSizeZ { get; set; }

        internal static Chunk[] Chunks_0_0;     //0.0
        internal static object Chunk_0_0_Lock = new object();
        internal static Chunk[] Chunks_1_0;     //-1.0
        internal static object Chunk_1_0_Lock = new object();
        internal static Chunk[] Chunks_0_1;     //0.-1
        internal static object Chunk_0_1_Lock = new object();
        internal static Chunk[] Chunks_1_1;     //-1.-1
        internal static object Chunk_1_1_Lock = new object();

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

            Chunks_0_0 = new Chunk[total_size];
            Chunks_1_0 = new Chunk[total_size];
            Chunks_0_1 = new Chunk[total_size];
            Chunks_1_1 = new Chunk[total_size];

            /*for (int z = 0; z < WorldSizeZ; z++)
            {
                for (int x = 0; x < WorldSizeX; x++)
                {
                    int z_offset = z * WorldSizeZ;
                    int index = x + z_offset;
                    Chunks[index] = new Chunk(x, z);
                }
            }*/     //1.604 sec

            for (int i = 0; i < total_size; i++)
            { 
                int z = (i / WorldSizeZ);
                int x = i - (z * WorldSizeZ);

                Chunks_0_0[i] = new Chunk(x, z);
            }

            for (int i = 0; i < total_size; i++)
            {
                int z = (i / WorldSizeZ);
                int x = i - (z * WorldSizeZ);

                Chunks_1_0[i] = new Chunk(-x - 1, z);
            }

            for (int i = 0; i < total_size; i++)
            {
                int z = (i / WorldSizeZ);
                int x = i - (z * WorldSizeZ);

                Chunks_0_1[i] = new Chunk(x, -z - 1);
            }

            for (int i = 0; i < total_size; i++)
            {
                int z = (i / WorldSizeZ);
                int x = i - (z * WorldSizeZ);

                Chunks_1_1[i] = new Chunk(-x - 1, -z - 1);
            }

            GC.Collect();
        }

        public static void SetBlock(Block block)
        {
            Position chunk_pos = block.GetChunkPosition();
            bool is_z_negative = chunk_pos.Z < 0;
            bool is_x_negative = chunk_pos.X < 0;

            int z_offset = (int)((chunk_pos.Z + (is_z_negative ? 1 : 0)) * WorldSizeZ);
            int chunk_index = z_offset + (int)(chunk_pos.X + (is_x_negative ? 1 : 0));

            if ((!is_x_negative && is_z_negative) || (is_x_negative && !is_z_negative))
                chunk_index = z_offset - (int)(chunk_pos.X + (is_x_negative ? 1 : 0));

            chunk_index = Math.Abs(chunk_index);

            if (chunk_pos.X >= 0 && chunk_pos.Z >= 0)
                lock (Chunk_0_0_Lock)
                    Chunks_0_0[chunk_index].SetBlock(block);
            else if (chunk_pos.X < 0 && chunk_pos.Z >= 0)
                lock (Chunk_1_0_Lock)
                    Chunks_1_0[chunk_index].SetBlock(block);
            else if (chunk_pos.X >= 0 && chunk_pos.Z < 0)
                lock (Chunk_0_1_Lock)
                    Chunks_0_1[chunk_index].SetBlock(block);
            else if (chunk_pos.X < 0 && chunk_pos.Z < 0)
                lock (Chunk_1_1_Lock)
                    Chunks_1_1[chunk_index].SetBlock(block);
            else
                Logger.Error("oopsie out of chunk range...");
        }

        public static void SetBlock(Position chunk_pos, Position local_block_chunk_pos, BlockType block_type)
        {
            int z_offset = (int)(chunk_pos.Z * WorldSizeZ);
            int chunk_index = z_offset + (int)chunk_pos.X;

            lock (Chunk_0_0_Lock)
            {
                Chunks_0_0[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            }
        }
        public static void SetBlock(Position chunk_pos, Position local_block_chunk_pos, ushort block_type)
        {
            bool is_z_negative = chunk_pos.Z < 0;
            bool is_x_negative = chunk_pos.X < 0;

            int z_offset = (int)((chunk_pos.Z + (is_z_negative ? 1 : 0)) * WorldSizeZ);
            int chunk_index = z_offset + (int)(chunk_pos.X + (is_x_negative ? 1 : 0));

            if ((!is_x_negative && is_z_negative) || (is_x_negative && !is_z_negative))
                chunk_index = z_offset - (int)(chunk_pos.X + (is_x_negative ? 1 : 0));

            chunk_index = Math.Abs(chunk_index);

            if (chunk_pos.X >= 0 && chunk_pos.Z >= 0)
                lock (Chunk_0_0_Lock)
                    Chunks_0_0[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            else if (chunk_pos.X < 0 && chunk_pos.Z >= 0)
                lock (Chunk_1_0_Lock)
                    Chunks_1_0[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            else if (chunk_pos.X >= 0 && chunk_pos.Z < 0)
                lock (Chunk_0_1_Lock)
                    Chunks_0_1[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            else if (chunk_pos.X < 0 && chunk_pos.Z < 0)
                lock (Chunk_1_1_Lock)
                    Chunks_1_1[chunk_index].SetBlock(chunk_pos, local_block_chunk_pos, block_type);
            else
                Logger.Error("oopsie out of chunk range...");
        }

        public static Chunk GetChunk(int x, int z)
        {
            bool is_z_negative = z < 0;
            bool is_x_negative = x < 0;

            int z_offset = (z + (is_z_negative ? 1 : 0)) * WorldSizeZ;
            int chunk_index = z_offset + (x + (is_x_negative ? 1 : 0));

            if ((!is_x_negative && is_z_negative) || (is_x_negative && !is_z_negative))
                chunk_index = z_offset - (x + (is_x_negative ? 1 : 0));

            chunk_index = Math.Abs(chunk_index);
            //Console.WriteLine($"x: {x} z: {z} index: {chunk_index}");

            /*lock (Chunk_0_0_Lock)
                return Chunks_0_0[chunk_index];
*/
            Position chunk_pos = new Position(x, 0, z);

            if (chunk_pos == new Position(1, 0, -2))
            { 
                Console.WriteLine(chunk_index);
            }

            if (chunk_pos.X >= 0 && chunk_pos.Z >= 0)
                lock (Chunk_0_0_Lock)
                    return Chunks_0_0[chunk_index];
            else if (chunk_pos.X < 0 && chunk_pos.Z >= 0)
                lock (Chunk_1_0_Lock)
                    return Chunks_1_0[chunk_index];
            else if (chunk_pos.X >= 0 && chunk_pos.Z < 0)
                lock (Chunk_0_1_Lock)
                    return Chunks_0_1[chunk_index];
            else if (chunk_pos.X < 0 && chunk_pos.Z < 0)
                lock (Chunk_1_1_Lock)
                    return Chunks_1_1[chunk_index];

            Logger.Error("oopsie out of chunk range...");
            return null;
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

        internal WorldManager LoadWorldFromFile(string path, int region_x = 0, int region_z = 0)
        {
            Logger.Log("Loading world...");
            Stopwatch stopwatch = Stopwatch.StartNew();
/*
            if (region_x == 0)
                region_x = 1;
            if (region_z == 0)
                region_z = 1;
*/
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
                        var level = chunkNbt.Get<NbtCompound>("Level");
                        var blocks = WorldLoader.ParseChunk(chunkNbt);

                        int chunk_z_pos = level.Get<NbtInt>("zPos").Value;
                        int chunk_x_pos = level.Get<NbtInt>("xPos").Value;
                        /*if (region_z < 0)
                            Console.WriteLine($"x: {chunk_x_pos} z: {chunk_z_pos}");*/
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

                                    //BlockType block_type = blocks[block_index];
                                    ushort block_type = blocks[block_index];
                                    /*Position block_pos = new Position(x + (chunk_x * _chunk_size_x), y, z + (chunk_z * _chunk_size_z));
                                    Block block = new Block()
                                        .SetBlockType(block_type)
                                        .SetPosition(block_pos);

                                    SetBlock(block);*/

                                    Position chunk_pos = new Position(chunk_x_pos, y_section, chunk_z_pos);
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
