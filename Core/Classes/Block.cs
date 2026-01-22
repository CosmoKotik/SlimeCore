using SlimeCore.Enums;
using SlimeCore.Structs;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Block
    {
        public Position Position { get; set; }
        public BlockType BlockType { get; set; } = BlockType.Empty;
        public short BlockID { get; set; }
        public byte Meta { get; set; }

        public static Block Empty = new Block() { BlockType = (int)BlockType.Air };

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
            //Logger.Error(value.ToString());
            return value;
        }

        public Block SetPosition(Position pos)
        {
            this.Position = pos;
            return this;
        }
        public Block SetBlockID(short id)
        {
            this.BlockID = id;
            return this;
        }
        public Block SetBlockType(BlockType type)
        {
            // Decoding: id = result >> 4; meta = result & 15
            this.BlockType = type;
            this.BlockID = (short)((int)type >> 4);
            this.Meta = (byte)((int)type & 15);
            return this;
        }
        public Block SetMeta(byte meta) 
        {
            this.Meta = meta;
            return this;
        }

        public Position GetChunkPosition()
        {
            Position absolute_pos = this.Position.Absolute();
            Position magnitude = this.Position.Magnitude();

            Position chunk_pos_absolute = absolute_pos / 16;
            Position chunk_pos = (chunk_pos_absolute * magnitude) + magnitude.Clamp(-1, 0);

            return chunk_pos;
        }
    }
}
