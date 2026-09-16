using System;
using SlimeCore.Network;

namespace SlimeCore.Core.Classes
{
	public class ConfirmTransaction
	{
		public byte WindowID { get; set; }
		public short ActionNumber { get; set; }
		public bool Accepted { get; set; }

		public ConfirmTransaction SetWindowID(byte value)
		{
			this.WindowID = value;
			return this;
		}

		public ConfirmTransaction SetActionNumber(short value)
		{
			this.ActionNumber = value;
			return this;
		}

		public ConfirmTransaction SetAccepted(bool value)
		{
			this.Accepted = value;
			return this;
		}

		public byte[] GetBytes()
		{
			BufferManager bm = new BufferManager();
			bm.WriteByte(this.WindowID);
			bm.WriteShort(this.ActionNumber);
			bm.WriteBool(this.Accepted);

			return bm.GetBytes();
		}
	}
}

