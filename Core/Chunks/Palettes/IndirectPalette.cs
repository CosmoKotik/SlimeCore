using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Palettes
{
    public class IndirectPalette : IPalette
    {
        public varint PaletteLength { get; set; }
        public List<varint> Palette { get; set; } = new List<varint>();

        public byte[] GetBytes()
        {
            BufferManager bm = new BufferManager();
            bm.WriteVarInt(PaletteLength);

            for (int i = 0; i < PaletteLength; i++)
                bm.WriteVarInt(Palette[i]);

            return bm.GetBytes();
        }
    }
}
