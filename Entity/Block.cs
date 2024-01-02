using SlimeCore.Enums;
using SlimeCore.Tools.Nbt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entity
{
    public class Block
    {
        /*public byte[] PackedXZCoords { get; set; }
        public short Height { get; set; }*/
        /*public BlockType BlockType { get; set; }
        public int BitsPerEntry { get; set; }

        public int X {  get; set; }
        public int Y {  get; set; }
        */

        public string Name { get; set; }
        public int ID { get; set; }

        public Block()
        { 
            
        }

        /*public Block(BlockType type, int x, int y)
        {
            this.BlockType = type;
            this.BitsPerEntry = GetBitsPerEntry(type);

            this.X = x;
            this.Y = y;
        }*/

        /*private int GetBitsPerEntry(BlockType type)
        {
            int bpe = 0;

            BitArray bits = new BitArray(BitConverter.GetBytes(BitsPerEntry));

            if (bits.Count < 4)
                bpe = 4;
            else
                bpe = bits.Count;
            
            Console.WriteLine(bits.Count);

            return bpe;
        }*/
    }
}
