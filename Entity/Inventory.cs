using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeCore.Entity
{
    public class Inventory
    {
        //private Dictionary<InventoryRegion, int[]> _slots;
        private List<InventoryRegion> _slots;

        public Inventory() 
        {
            //_slots = new Dictionary<InventoryRegion, int[]>();
            _slots = new List<InventoryRegion>();
        }

        public Inventory Add(string name, int from, int to)
        {
            InventoryRegion region = new InventoryRegion(name, from, to, new int[to - from + 1]);
            _slots.Add(region);
            return this;
        }

        public Inventory SetItem(string name, int slotIndex, int itemID)
        {
            _slots.Find(x => x.Name == name).Items[slotIndex] = itemID;
            return this;
        }

        public Inventory SetItem(int globalSlotIndex, int itemID)
        {
            InventoryRegion region = _slots.Find(x => globalSlotIndex >= x.From && globalSlotIndex <= x.To);
            region.Items[globalSlotIndex - region.From] = itemID;
            return this;
        }

        public int GetItem(int globalSlotIndex)
        {
            InventoryRegion region = _slots.Find(x => globalSlotIndex >= x.From && globalSlotIndex <= x.To);
            return region.Items[globalSlotIndex - region.From];
        }

        public int GetItem(string name, int slotIndex)
        {
            return _slots.Find(x => x.Name.Equals(name)).Items[slotIndex];
        }
    }

    public class InventoryRegion
    { 
        public int From { get; set; }
        public int To { get; set; }
        public string Name { get; set; }
        public int[] Items { get; set; }

        public InventoryRegion(string name, int from, int to, int[] items) 
        { 
            this.From = from;
            this.To = to;
            this.Name = name;
            this.Items = items;
        }
    }
}
