using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Generation
{
    /// <summary>
    /// Vanilla-style gradient noise ("Improved" Perlin).
    /// This is the core used by octave generators.
    /// </summary>
    public sealed class NoiseGeneratorImproved
    {
        private readonly int[] _p = new int[512];
        public double XCoord { get; }
        public double YCoord { get; }
        public double ZCoord { get; }

        public NoiseGeneratorImproved(JavaRandom rand)
        {
            XCoord = rand.NextDouble() * 256.0;
            YCoord = rand.NextDouble() * 256.0;
            ZCoord = rand.NextDouble() * 256.0;

            int[] perm = new int[256];
            for (int i = 0; i < 256; i++) perm[i] = i;

            for (int i = 0; i < 256; i++)
            {
                int j = rand.NextInt(256 - i) + i;
                (perm[i], perm[j]) = (perm[j], perm[i]);
            }

            for (int i = 0; i < 512; i++) _p[i] = perm[i & 255];
        }

        private static double Fade(double t) => t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
        private static double Lerp(double t, double a, double b) => a + t * (b - a);

        private static double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public double Noise(double x, double y, double z)
        {
            x += XCoord; y += YCoord; z += ZCoord;

            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            double u = Fade(x);
            double v = Fade(y);
            double w = Fade(z);

            int A = _p[X] + Y;
            int AA = _p[A] + Z;
            int AB = _p[A + 1] + Z;

            int B = _p[X + 1] + Y;
            int BA = _p[B] + Z;
            int BB = _p[B + 1] + Z;

            double res =
                Lerp(w,
                    Lerp(v,
                        Lerp(u, Grad(_p[AA], x, y, z), Grad(_p[BA], x - 1, y, z)),
                        Lerp(u, Grad(_p[AB], x, y - 1, z), Grad(_p[BB], x - 1, y - 1, z))
                    ),
                    Lerp(v,
                        Lerp(u, Grad(_p[AA + 1], x, y, z - 1), Grad(_p[BA + 1], x - 1, y, z - 1)),
                        Lerp(u, Grad(_p[AB + 1], x, y - 1, z - 1), Grad(_p[BB + 1], x - 1, y - 1, z - 1))
                    )
                );

            return res;
        }

        public void PopulateNoiseArray(
            double[] buffer,
            int xStart, int yStart, int zStart,
            int xSize, int ySize, int zSize,
            double xScale, double yScale, double zScale)
        {
            int idx = 0;

            for (int x = 0; x < xSize; x++)
            {
                double nx = (xStart + x) * xScale;
                for (int z = 0; z < zSize; z++)
                {
                    double nz = (zStart + z) * zScale;
                    for (int y = 0; y < ySize; y++)
                    {
                        double ny = (yStart + y) * yScale;
                        buffer[idx++] += Noise(nx, ny, nz);
                    }
                }
            }
        }
    }
}
