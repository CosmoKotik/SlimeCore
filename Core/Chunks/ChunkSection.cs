using SlimeCore.Core.Chunks.Palettes;
using SlimeCore.Entity;
using SlimeCore.Enums;
using SlimeCore.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks
{
    public class ChunkSection
    {
        public short BlockCount { get; set; }
        public PalettedContainer BlockStates { get; set; }
        public PalettedContainer Biomes { get; set; }

        public void Fill(BlockType type)
        {
            //Block block = new Block(type, 0, 0);

            IndirectPalette palette = new IndirectPalette()
            {
                IDs = new int[] { (byte)BlockType.Block, (byte)type }
            };

            long[] blockEntries = new long[((64 / 4) * 16)];
            for (int i = 0; i < blockEntries.Length; i++)
            {
                long cum = 0;
                //Console.WriteLine($"Shifted byte: {Convert.ToString(cum, toBase: 2)}");
                for (int n = 0; n < 16; n++)
                {
                    cum = (cum << 4);
                    for (int m = 0; m < (int)type; m++) 
                    {
                        cum += 1;
                    }
                }

                blockEntries[i] = cum;
                BlockCount++;
            }

            int bitPerEntry = 4;

            BlockStates = new PalettedContainer()
            {
                //BitsPerEntry = block.BitsPerEntry,
                BitsPerEntry = bitPerEntry,
                Palette = palette,
                Indices = blockEntries
            };

            Biomes = new PalettedContainer()
            {
                BitsPerEntry = 0,
                Palette = new IndirectPalette() { IDs = new int[1] { 0 } },
                Indices = new long[0]
            };
        }

        public byte[] GetBytes() 
        {
            BufferManager bm = new BufferManager();
            bm.AddShort(BlockCount);

            bm.AddByte((byte)BlockStates.BitsPerEntry);

            IndirectPalette palette = (IndirectPalette)BlockStates.Palette;

            bm.AddVarInt(palette.IDs.Length);
            for (int i = 0; i < palette.IDs.Length; i++)
                bm.AddVarInt(palette.IDs[i]);

            //bm.AddVarInt((BlockStates.Indices.Length * BlockStates.BitsPerEntry) / 64);
            bm.AddVarInt(BlockStates.Indices.Length);
            for (int i = 0; i < BlockStates.Indices.Length; i++)
                bm.AddLong(BlockStates.Indices[i]);


            //Biome
            bm.AddByte((byte)0);
            bm.AddVarInt(15);
            bm.AddVarInt(0);


            bm.AddByte(0);
            bm.AddByte(0);
            bm.AddByte(0);
            bm.AddByte(0);

            bm.AddVarInt(0);
            bm.AddVarInt(0);

            //bm.AddVarInt(2);
            //bm.AddVarInt(0x24);
            //bm.AddVarInt(0x0F);

            //bm.AddVarInt(1);
            //bm.AddLong(4147);
            //bm.AddBytes(StringToByteArray("01 02 24 0F 01 33 10 00 00 00 00 00 00"), false);
            /*for (int i = 0; i < BlockStates.Indices.Length; i++)
                bm.AddLong(BlockStates.Indices[i]);*/

            return bm.GetBytes();
        }

        private static byte[] StringToByteArray(string hexc)
        {
            string[] hexValuesSplit = hexc.Split(' ');
            byte[] bytes = new byte[hexValuesSplit.Length];
            int i = 0;
            foreach (string hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}",
                //                    hex, value, stringValue, charValue);
                bytes[i] = (byte)value;
                i++;
            }

            return bytes;
        }
    }
}
