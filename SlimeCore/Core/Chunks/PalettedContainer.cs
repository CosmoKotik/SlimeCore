using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeCore.Enums;

namespace SlimeCore.Core.Chunks
{
    public class PalettedContainer
    {
        public int BitsPerEntry { get; set; }
        public IPalette Palette { get; set; }
        public long[] Indices { get; set; }
    }
}
