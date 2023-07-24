using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.World
{
    public interface IPaletteContainer
    {
        public byte BitsPerEntry { get; }
        public IPaletteContainer Palette { get; }
        public int DataArrayLength { get; set; }
        public long[] DataArray { get; set; }
    }
}
