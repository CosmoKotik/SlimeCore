using System;
using SlimeCore.Network;

namespace SlimeCore.Core.Classes
{
	public class ClickWindow
	{
		public byte WindowID { get; set; }
		public short Slot { get; set; }
		public byte Button { get; set; }
		public short ActionNumber { get; set; }
		public byte Mode { get; set; }
		public Slot ClickedItem { get; set; }

		public ClickWindow()
		{
		}

		public ClickWindow SetWindowID(byte value)
		{
			this.WindowID = value;
			return this;
		}

		public ClickWindow SetSlot(short value)
		{
			this.Slot = value;
			return this;
		}

		public ClickWindow SetButton(byte value)
		{
			this.Button = value;
			return this;
		}

		public ClickWindow SetActionNumber(short value)
		{
			this.ActionNumber = value;
			return this;
		}

		public ClickWindow SetMode(byte value)
		{
			this.Mode = value;
			return this;
		}

		public ClickWindow SetClickedItem(Slot value)
		{
			this.ClickedItem = value;
			return this;
		}

		public byte[] GetBytes()
		{
			BufferManager bm = new BufferManager();

			bm.WriteByte(this.WindowID);
			bm.WriteShort(this.Slot);
			bm.WriteByte(this.Button);
			bm.WriteShort(this.ActionNumber);
			bm.WriteByte(this.Mode);
			bm.WriteBytes(this.ClickedItem.GetBytes(), false);

			return bm.GetBytes();
		}
	}
}

