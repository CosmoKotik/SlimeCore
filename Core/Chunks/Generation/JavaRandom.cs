using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Generation
{
    /// <summary>
    /// java.util.Random compatible (48-bit LCG).
    /// Required if you want vanilla-deterministic behavior.
    /// </summary>
    public sealed class JavaRandom
    {
        private const long Multiplier = 0x5DEECE66DL; // 25214903917
        private const long Addend = 0xBL;             // 11
        private const long Mask = (1L << 48) - 1;

        private long _seed;

        public JavaRandom(long seed) => SetSeed(seed);

        public void SetSeed(long seed)
        {
            _seed = (seed ^ Multiplier) & Mask;
        }

        private int NextBits(int bits)
        {
            _seed = (_seed * Multiplier + Addend) & Mask;
            return (int)((ulong)_seed >> (48 - bits));
        }

        public int NextInt()
        {
            return NextBits(32);
        }

        public int NextInt(int bound)
        {
            if (bound <= 0) throw new ArgumentOutOfRangeException(nameof(bound));

            // Power of two fast path
            if ((bound & -bound) == bound)
                return (int)((bound * (long)NextBits(31)) >> 31);

            int bits, val;
            do
            {
                bits = NextBits(31);
                val = bits % bound;
            } while (bits - val + (bound - 1) < 0);

            return val;
        }

        public long NextLong()
        {
            long hi = (long)NextBits(32);
            long lo = (long)NextBits(32) & 0xffffffffL;
            return (hi << 32) | lo;
        }

        public double NextDouble()
        {
            long a = ((long)NextBits(26) << 27) + NextBits(27);
            return a / (double)(1L << 53);
        }

        public float NextFloat()
        {
            return NextBits(24) / ((float)(1 << 24));
        }
    }
}
