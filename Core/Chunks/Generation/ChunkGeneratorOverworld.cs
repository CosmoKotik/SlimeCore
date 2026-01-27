using SlimeCore.Enums;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Generation
{
    /// <summary>
    /// Vanilla-like 1.12.2 overworld generator skeleton:
    /// - builds 5x5x33 density grid
    /// - interpolates to chunk blocks
    /// - hooks for biomes, surface replacement, caves/ravines, population
    /// </summary>
    public sealed class ChunkGeneratorOverworld
    {
        private readonly long _worldSeed;
        private readonly int _seaLevel = 63;

        // Noise gens (ordering matters if you want to match vanilla determinism)
        private readonly NoiseGeneratorOctaves _minLimit;
        private readonly NoiseGeneratorOctaves _maxLimit;
        private readonly NoiseGeneratorOctaves _main;
        private readonly NoiseGeneratorOctaves _depth;

        // Working buffers
        private double[]? _minLimitBuf;
        private double[]? _maxLimitBuf;
        private double[]? _mainBuf;
        private double[]? _depthBuf;

        public ChunkGeneratorOverworld(long worldSeed)
        {
            _worldSeed = worldSeed;

            // One JavaRandom, in vanilla init order.
            var rand = new JavaRandom(worldSeed);

            _minLimit = new NoiseGeneratorOctaves(rand, 16);
            _maxLimit = new NoiseGeneratorOctaves(rand, 16);
            _main = new NoiseGeneratorOctaves(rand, 8);
            _depth = new NoiseGeneratorOctaves(rand, 16);
        }

        public Chunk GenerateChunk(int chunkX, int chunkZ)
        {
            //var chunk = new Chunk(chunkX, chunkZ);
            var chunk = WorldManager.GetChunk(chunkX, chunkZ);

            // 1) Biomes (stub: set plains everywhere)
            FillBiomesStub(chunk);

            // 2) Base terrain (stone/air/water + bedrock)
            BuildBaseTerrain(chunkX, chunkZ, chunk);

            // 3) Surface replacement (stub; make it grass/dirt on top)
            ReplaceSurfaceStub(chunk);

            // 4) Carvers (stubs)
            // CarveCaves(chunkX, chunkZ, chunk);
            // CarveRavines(chunkX, chunkZ, chunk);

            chunk.RebuildHeightmap();
            return chunk;
        }

        private void FillBiomesStub(Chunk chunk)
        {
            // Vanilla would generate biomes using GenLayer chain.
            // For now: Plains (ID 1) everywhere
            const byte Plains = 1;
            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                    chunk.Biomes[x, z] = Plains;
        }

        private void BuildBaseTerrain(int chunkX, int chunkZ, Chunk chunk)
        {
            // Generate density grid (5x5x33)
            double[,,] density = GenerateDensity(chunkX, chunkZ, chunk.Biomes);

            // Fill blocks by interpolating each cell to 4x8x4 subcells
            for (int cellX = 0; cellX < 4; cellX++)
                for (int cellZ = 0; cellZ < 4; cellZ++)
                    for (int cellY = 0; cellY < 32; cellY++)
                    {
                        // corners in density grid
                        double d000 = density[cellX, cellY, cellZ];
                        double d001 = density[cellX, cellY, cellZ + 1];
                        double d010 = density[cellX, cellY + 1, cellZ];
                        double d011 = density[cellX, cellY + 1, cellZ + 1];

                        double d100 = density[cellX + 1, cellY, cellZ];
                        double d101 = density[cellX + 1, cellY, cellZ + 1];
                        double d110 = density[cellX + 1, cellY + 1, cellZ];
                        double d111 = density[cellX + 1, cellY + 1, cellZ + 1];

                        for (int subY = 0; subY < 8; subY++)
                        {
                            double fy = subY / 8.0;

                            double d00 = Lerp(fy, d000, d010);
                            double d01 = Lerp(fy, d001, d011);
                            double d10 = Lerp(fy, d100, d110);
                            double d11 = Lerp(fy, d101, d111);

                            for (int subX = 0; subX < 4; subX++)
                            {
                                double fx = subX / 4.0;
                                double d0 = Lerp(fx, d00, d10);
                                double d1 = Lerp(fx, d01, d11);

                                for (int subZ = 0; subZ < 4; subZ++)
                                {
                                    double fz = subZ / 4.0;
                                    double dens = Lerp(fz, d0, d1);

                                    int x = cellX * 4 + subX;
                                    int y = cellY * 8 + subY;
                                    int z = cellZ * 4 + subZ;

                                    int section_y = y / 16;
                                    int block_y = y - (section_y * 16);
                                    Position chunk_pos = new Position(chunkX, section_y, chunkZ);
                                    Position local_block_pos = new Position(x, block_y, z);

                                    //Console.WriteLine($"{section_y} {block_y}");


                                    if (y == 0)
                                    {
                                        chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Bedrock);
                                        continue;
                                    }

                                    if (dens > 0.0)
                                    {
                                        chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Stone);
                                    }
                                    else if (y < _seaLevel)
                                    {
                                        chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Water);
                                    }
                                    else
                                    {
                                        chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Air);
                                    }
                                }
                            }
                        }
                    }
        }

        private double[,,] GenerateDensity(int chunkX, int chunkZ, byte[,] biomes16)
        {
            // Vanilla samples at 5x5x33 starting at (chunkX*4, chunkZ*4) in "noise space"
            int xStart = chunkX * 4;
            int zStart = chunkZ * 4;

            // ---- Common vanilla-ish scales (you may need to verify for perfect match) ----
            // These values are widely used in decompiled 1.12.2-style implementations.
            double xzScale = 684.412;
            double yScale = 684.412;
            double xzMainScale = 684.412 / 80.0;
            double yMainScale = 684.412 / 160.0;

            // Generate buffers
            // min/max/main are 3D noises over 5x33x5
            _minLimitBuf = _minLimit.GenerateNoiseOctaves(_minLimitBuf, xStart, 0, zStart, 5, 33, 5, xzMainScale, yMainScale, xzMainScale);
            _maxLimitBuf = _maxLimit.GenerateNoiseOctaves(_maxLimitBuf, xStart, 0, zStart, 5, 33, 5, xzMainScale, yMainScale, xzMainScale);
            _mainBuf = _main.GenerateNoiseOctaves(_mainBuf, xStart, 0, zStart, 5, 33, 5, xzScale / 80.0, yScale / 160.0, xzScale / 80.0);

            // depthNoise is 2D in vanilla; we’ll fake it by sampling ySize=1 and indexing
            _depthBuf = _depth.GenerateNoiseOctaves(_depthBuf, xStart, 0, zStart, 5, 1, 5, 200.0, 1.0, 200.0);

            double[,,] outGrid = new double[5, 33, 5];

            // Vanilla also blends biome depth/scale using a 5x5 neighborhood and a precomputed weight kernel.
            // To keep this skeleton simple-but-vanilla-ish, we use a fixed baseHeight & variation for now.
            // Replace this with real biome depth/scale blending once you implement GenLayer biomes.
            double baseHeight = 0.125;   // vanilla default-ish
            double heightVar = 0.05;    // vanilla default-ish

            int i = 0;
            int d = 0;

            for (int x = 0; x < 5; x++)
                for (int z = 0; z < 5; z++)
                {
                    // Depth term (2D)
                    double depthVal = _depthBuf![d++] / 8000.0;
                    if (depthVal < 0) depthVal = -depthVal * 0.3;
                    depthVal = depthVal * 3.0 - 2.0;
                    if (depthVal < 0) depthVal /= 2.0;

                    double depth = baseHeight + depthVal * 0.2;
                    double scale = heightVar + depthVal * 0.01;
                    if (scale < 0.0) scale = 0.0;

                    for (int y = 0; y < 33; y++)
                    {
                        double mainVal = _mainBuf![i] / 10.0 + 1.0;
                        double alpha = Clamp(mainVal / 2.0, 0.0, 1.0);

                        double minVal = _minLimitBuf![i] / 512.0;
                        double maxVal = _maxLimitBuf![i] / 512.0;

                        double limit = Lerp(alpha, minVal, maxVal);

                        // Vertical shaping: pushes terrain down/up with height
                        double yNorm = (y - 16.0) * 8.0; // vanilla works in 8-block steps
                        double density = limit - yNorm * (1.0 / (scale * 8.0 + 1e-6)) + depth * 8.0;

                        // Simple top/bottom slides (vanilla has configurable slides)
                        density = ApplySlides(density, y);

                        outGrid[x, y, z] = density;
                        i++;
                    }
                }

            return outGrid;
        }

        private static double ApplySlides(double density, int yIndex)
        {
            // Vanilla has "topSlide" and "bottomSlide" to smooth extremes.
            // This is a light approximation; tune later.
            if (yIndex > 29)
            {
                double t = (yIndex - 29) / 3.0;
                density = Lerp(t, density, -10.0);
            }
            if (yIndex < 3)
            {
                double t = (3 - yIndex) / 3.0;
                density = Lerp(t, density, -10.0);
            }
            return density;
        }

        private void ReplaceSurfaceStub(Chunk chunk)
        {
            // Vanilla: replaceBiomeBlocks() uses biome topBlock/fillerBlock and surface noise.
            // Stub: grass on top, 3 dirt below, then stone.
            int section_y, block_y;
            Position chunk_pos, local_block_pos;

            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                {
                    int top = -1;
                    for (int y = 255; y >= 0; y--)
                    {
                        section_y = y / 16;
                        block_y = y - (section_y * 16);
                        chunk_pos = new Position(chunk.ChunkX, section_y, chunk.ChunkZ);
                        local_block_pos = new Position(x, block_y, z);

                        var b = chunk.GetBlock(chunk_pos, local_block_pos);
                        if (b.BlockType != BlockType.Air && b.BlockType != BlockType.Water)
                        {
                            top = y;
                            break;
                        }
                    }
                    if (top <= 0) continue;

                    section_y = top / 16;
                    block_y = top - (section_y * 16);
                    chunk_pos = new Position(chunk.ChunkX, section_y, chunk.ChunkZ);
                    local_block_pos = new Position(x, block_y, z);

                    // If exposed above water, grass
                    if (top >= _seaLevel - 1)
                        chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Grass);

                    for (int k = 1; k <= 3; k++)
                    {
                        int y = top - k;
                        if (y <= 0) break;
                        if (chunk.GetBlock(chunk_pos, local_block_pos).BlockType == BlockType.Stone)
                            chunk.SetBlock(chunk_pos, local_block_pos, BlockType.Dirt);
                    }
                }
        }

        private static double Lerp(double t, double a, double b) => a + t * (b - a);
        private static double Clamp(double v, double lo, double hi) => v < lo ? lo : (v > hi ? hi : v);
    }
}
