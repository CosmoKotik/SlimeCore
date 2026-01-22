using fNbt;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Loader
{
    public class WorldLoader
    {
        public static NbtCompound LoadChunkNBT(byte[] data)
        {
            var nbt = new NbtFile();

            using (var ms = new MemoryStream(data))
            { 
                nbt.LoadFromStream(ms, NbtCompression.None, tag => true);
                return nbt.RootTag;
            }
        }

        public static BlockType[] ParseChunk(NbtCompound chunk)
        {
            var level = chunk.Get<NbtCompound>("Level");
            var sections = level.Get<NbtList>("Sections");

            BlockType[] blocks = new BlockType[16 * 256 * 16];

            for (int section_index = 0; section_index < sections.Count; section_index++)
            {
                NbtCompound section = sections[section_index] as NbtCompound;

                int yBase = section.Get<NbtByte>("Y").Value * 16;

                byte[] ids = section.Get<NbtByteArray>("Blocks").Value;
                byte[] meta = section.Get<NbtByteArray>("Data").Value;

                for (int i = 0; i < 4096; i++)
                {
                    int x = i & 15;
                    int z = (i >> 4) & 15;
                    int y = (i >> 8) & 15;

                    int blockId = ids[i] & 0xFF;
                    int meta_value = (i & 1) == 0 ? meta[i >> 1] & 0x0F : (meta[i >> 1] >> 4) & 0x0F;

                    int encoded = (blockId << 4) | meta_value;

                    blocks[i + (yBase * 256)] = (BlockType)encoded;
                }
            }

            /*foreach (NbtCompound section in sections)
            {
                int yBase = section.Get<NbtByte>("Y").Value * 16;

                byte[] ids = section.Get<NbtByteArray>("Blocks").Value;
                byte[] meta = section.Get<NbtByteArray>("Data").Value;

                for (int i = 0; i < 4096; i++)
                {
                    int x = i & 15;
                    int z = (i >> 4) & 15;
                    int y = (i >> 8) & 15;

                    int blockId = ids[i] & 0xFF;
                    int meta_value = (i & 1) == 0 ? meta[i >> 1] & 0x0F : (meta[i >> 1] >> 4) & 0x0F;

                    int encoded = (blockId << 4) | meta_value;

                    blocks[i + (yBase * 256)] = (BlockType)encoded; 
                }

            }*/

            return blocks;
        }
    }
}
