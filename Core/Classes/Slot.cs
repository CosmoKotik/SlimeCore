using SlimeCore.Enums;
using SlimeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Slot
    {
        public int SlotID { get; set; }
        public SlotType SlotType { get; set; }
        public BlockType BlockType { get; set; } = BlockType.Empty;
        public int BlockID { get; set; }
        public int ItemMeta { get; set; }
        public byte ItemCount { get; set; }
        public short ItemDamage { get; set; }

        public bool IsItem { get; set; }

        //NBT not implemented

        public Slot() { }
        public Slot(int slotID, SlotType type)
        { 
            this.SlotID = slotID;
            this.SlotType = type;
        }

        public Slot SetBlockType(BlockType type)
        {
            // Decoding: id = result >> 4; meta = result & 15

            this.BlockType = type;
            
            int id = (int)type >> 4;
            int meta = (int)type & 15;

            this.BlockID = id;
            this.ItemMeta = meta;

            return this;
        }
        public Slot SetBlockID(int id) { this.BlockID = id; return this; }
        public Slot SetItemMeta(int meta) { this.ItemMeta = meta; return this; }
        public Slot SetItemCount(byte count) { this.ItemCount = count; return this; }
        public Slot SetItemDamage(short damage) { this.ItemDamage = damage; return this; }
        public Slot SetSlotID(int slotID) { this.SlotID = slotID; return this; }
        public Slot SetSlotType(SlotType type) { this.SlotType = type; return this; }

        public byte[] GetBytes()
        {
            BufferManager bm = new BufferManager();

            if (this.BlockType.Equals(BlockType.Empty))
            {
                bm.WriteShort((int)BlockType.Empty_No_Encoding);
                return bm.GetBytes();
            }

            bm.WriteShort((short)this.BlockID);
            bm.WriteByte(this.ItemCount);
            if (this.IsItem)
                bm.WriteShort(this.ItemDamage);
            else
                bm.WriteShort((short)this.ItemMeta);

            bm.WriteByte(0x00); //nbt

            return bm.GetBytes();
        }
    }
}
