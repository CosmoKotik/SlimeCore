using SlimeCore.Enums;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Block
    {
        public Position Position { get; set; }
        public int BlockID { get; set; }
        public int Meta { get; set; }

        public static Block Empty = new Block() { BlockID = (int)BlockType.Air };

        /*public varint GetIDWithMeta()
        {
            long value = this.BlockID << 4 | (this.Meta & 15);
            return (varint)value;
        }*/
        public int GetIDWithMeta()
        {
            int value = this.BlockID << 4 | (this.Meta & 15);
            return value;
        }

        public Block SetPosition(Position pos)
        {
            this.Position = pos;
            return this;
        }
        public Block SetBlockType(BlockType type)
        {
            this.BlockID = (int)type;
            return this;
        }
        public Block SetMeta(int meta) 
        {
            this.Meta = meta;
            return this;
        }
    }
}
