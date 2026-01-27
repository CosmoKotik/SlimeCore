using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Generation
{
    /// <summary>
    /// Vanilla octave noise: sums multiple Improved noise layers.
    /// </summary>
    public sealed class NoiseGeneratorOctaves
    {
        private readonly NoiseGeneratorImproved[] _generators;
        private readonly int _octaves;

        public NoiseGeneratorOctaves(JavaRandom rand, int octaves)
        {
            _octaves = octaves;
            _generators = new NoiseGeneratorImproved[octaves];
            for (int i = 0; i < octaves; i++)
                _generators[i] = new NoiseGeneratorImproved(rand);
        }

        public double[] GenerateNoiseOctaves(
            double[]? buffer,
            int xStart, int yStart, int zStart,
            int xSize, int ySize, int zSize,
            double xScale, double yScale, double zScale)
        {
            int len = xSize * ySize * zSize;
            buffer ??= new double[len];
            Array.Clear(buffer, 0, len);

            double freq = 1.0;
            for (int i = 0; i < _octaves; i++)
            {
                double xs = xScale * freq;
                double ys = yScale * freq;
                double zs = zScale * freq;

                // Vanilla scales amplitude by 1/freq for octaves
                // but the "Improved" populate adds directly; we apply amplitude by multiplying later if needed.
                // Common approach: divide input scale as freq increases, and multiply output by 1/freq.
                double amp = 1.0 / freq;

                // temp array accumulate then scale
                double[] tmp = new double[len];
                _generators[i].PopulateNoiseArray(tmp, xStart, yStart, zStart, xSize, ySize, zSize, xs, ys, zs);

                for (int j = 0; j < len; j++)
                    buffer[j] += tmp[j] * amp;

                freq *= 2.0;
            }

            return buffer;
        }
    }
}
