using SlimeCore.Core.Utils;
using SlimeCore.Core.Utils.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.World
{
    public class ChunkSection : IPaletteContainer
    {
        public byte BitsPerEntry { get => 4; }
        public IPaletteContainer Palette { get => this; }
        public int DataArrayLength { get; set; }
        public long[] DataArray { get; set; }

        public ChunkSection() 
        { 
            
        }

        public byte[] Get(int size, bool isBiome = false)
        {
            BufferManager bm = new BufferManager();
            List<byte> buffer = new List<byte>();

            //Bits per block
            bm.WriteByte(4, ref buffer);

            //Palette Length
            bm.WriteVarInt((size * 4) / 64, ref buffer);

            for (int i = 0; i < (size * 4) / 64; i++)
            {
                bm.WriteVarInt(1, ref buffer);
            }

            //Data Array Length
            bm.WriteVarInt((size * 4)/64, ref buffer);

            //Data Array
            for (int y = 0; y < 16; y++)
                for (int z = 0; z < 16; z++)
                    for (int x = 0; x < 16; x++)
                    {
                        //bm.WriteLong(long.Parse(Convert.ToString((long)0, 2)), ref buffer);
                        bm.WriteByte(0, ref buffer);
                    }
            //bm.WriteLong(0, ref buffer);

            //Block light
            for (int i = 0; i < size; i++)
            {
                bm.WriteByte(0, ref buffer);
            }

            //Skylight
            for (int i = 0; i < size; i++)
            {
                bm.WriteByte(0, ref buffer);
            }

            return buffer.ToArray();
        }
    }
}
