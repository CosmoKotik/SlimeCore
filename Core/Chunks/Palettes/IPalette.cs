using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Palettes
{
    public interface IPalette
    {
        public varint PaletteLength { get; set; }

        public byte[] GetBytes();
    }
}
