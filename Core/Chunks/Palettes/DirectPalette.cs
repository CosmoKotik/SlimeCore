using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Palettes
{
    public class DirectPalette : IPalette
    {
        public varint PaletteLength { get; set; } = 0;

        public byte[] GetBytes()
        {
            BufferManager bm = new BufferManager();
            bm.WriteVarInt(PaletteLength);

            return bm.GetBytes();
        }
    }
}
