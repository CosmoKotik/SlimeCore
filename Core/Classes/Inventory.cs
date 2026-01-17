using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class Inventory
    {
        public Slot[] Slots { get; set; }

        public bool IsInitialized { get; set; }

        public Inventory()
        {
            this.Slots = new Slot[45];

            for (int i = 0; i < this.Slots.Length; i++)
            {
                if (i == 0)
                    this.Slots[i] = new Slot(i, SlotType.CRAFTING_OUTPUT);
                else if (i >= 1 && i <= 4)
                    this.Slots[i] = new Slot(i, SlotType.CRAFTING_INPUT);
                else if (i >= 5 && i <= 8)
                    this.Slots[i] = new Slot(i, SlotType.ARMOR);
                else if (i >= 9 && i <= 35)
                    this.Slots[i] = new Slot(i, SlotType.MAIN_INVENTORY);
                else if (i >= 36 && i <= 44)
                    this.Slots[i] = new Slot(i, SlotType.HOTBAR);
                else if (i == 45)
                    this.Slots[i] = new Slot(i, SlotType.OFFHAND);

                //ik shitcode but idk how to make it nicer
            }
        }

        public Inventory SetBlockToSlot(BlockType block, int SlotID)
        {
            this.Slots[SlotID].SetBlockType(block);
            return this;
        }
        public Inventory UpdateSlot(Slot slot)
        {
            int slotID = slot.SlotID;
            int blockID = slot.BlockID;
            int metadata = slot.ItemDamage;
            BlockType block = (BlockType)(blockID << 4 | (metadata & 15));

            this.Slots[slotID].SetBlockType(block);

            return this;
        }
        public Slot GetSlotFromID(int slot_id)
        {
            return this.Slots[slot_id];
        }
        public BlockType GetBlockTypeFromSlotID(int slot_id)
        {
            return this.Slots[slot_id].BlockType;
        }
    }
}
