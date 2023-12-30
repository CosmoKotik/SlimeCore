using SlimeCore.Tools.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks
{
    public class Chunk
    {
        public int ChunkX { get; set; }
        public int ChunkY { get; set; }
        public Nbt Heightmaps { get; set; }
        
        public ChunkSection[] Data { get; set; } = new ChunkSection[16];
    }
}
