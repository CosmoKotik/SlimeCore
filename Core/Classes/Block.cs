using SlimeCore.Enums;
using SlimeCore.Structs;
using SlimeCore.Tools;
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
        public BlockType BlockType { get; set; } = BlockType.Empty;
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
            int value = (int)this.BlockType;
            if (this.BlockType.Equals(BlockType.Empty))
                value = this.BlockID << 4 | (this.Meta & 15);
            Logger.Error(value.ToString());
            return value;
        }

        public Block SetPosition(Position pos)
        {
            this.Position = pos;
            return this;
        }
        public Block SetBlockID(int id)
        {
            this.BlockID = id;
            return this;
        }
        public Block SetBlockType(BlockType type)
        {
            // Decoding: id = result >> 4; meta = result & 15
            this.BlockType = type;
            this.BlockID = (int)type >> 4;
            this.Meta = (int)type & 15;
            return this;
        }
        public Block SetMeta(int meta) 
        {
            this.Meta = meta;
            return this;
        }
    }
}
