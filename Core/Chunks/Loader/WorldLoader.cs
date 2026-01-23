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

        public static ushort[] ParseChunk(NbtCompound level)
        {
            //Before => 182mb
            //Now => 179mb & 1.588 sec

            //var level = chunk.Get<NbtCompound>("Level");
            var sections = level.Get<NbtList>("Sections");

            //BlockType[] blocks = new BlockType[16 * 256 * 16];
            ushort[] blocks = new ushort[16 * 256 * 16];

            for (int section_index = 0; section_index < sections.Count; section_index++)
            {
                NbtCompound section = sections[section_index] as NbtCompound;

                int yBase = section.Get<NbtByte>("Y").Value * 16;

                byte[] ids = section.Get<NbtByteArray>("Blocks").Value;
                byte[] meta = section.Get<NbtByteArray>("Data").Value;

                for (int i = 0; i < 4096; i++)
                {
                    /*int x = i & 15;
                    int z = (i >> 4) & 15;
                    int y = (i >> 8) & 15;
*/
                    ushort blockId = (ushort)(ids[i] & 0xFF);
                    byte meta_value = (byte)((i & 1) == 0 ? meta[i >> 1] & 0x0F : (meta[i >> 1] >> 4) & 0x0F);

                    ushort encoded = (ushort)((blockId << 4) | meta_value);

                    blocks[i + (yBase * 256)] = encoded;
                }
            }

            return blocks;
        }
    }
}
