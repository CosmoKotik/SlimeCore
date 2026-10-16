using System;
using SlimeCore.Network;
using SlimeCore.Structs;

namespace SlimeCore.Core.Classes
{
	public class MultiBlock
	{
		public int ChunkX { get; private set; }
		public int ChunkZ { get; private set; }

		public int RecordCount { get; private set; }

		// Records
		public List<MultiBlockRecord> Records { get; set; }


		public MultiBlock()
		{
			this.Records = new List<MultiBlockRecord>();

			this.RecordCount = 0;
		}

		public MultiBlock SetChunk(int x, int z)
		{
			this.ChunkX = x;
			this.ChunkZ = z;

			return this;
		}

		public MultiBlock AddRecord(
			byte horizontalPosition,
			byte yCoordinate,
			int blockID)
		{
			MultiBlockRecord record = new MultiBlockRecord(
				horizontalPosition,
				yCoordinate,
				blockID);

			this.Records.Add(record);
			this.RecordCount++;

			return this;
		}

		public MultiBlock AddRecord(
			byte xCoordinate,
			byte zCoordinate,
			byte yCoordinate,
			int blockID)
		{
			byte horizontalPosition = (byte)((xCoordinate << 4) | (zCoordinate & 0x0F));

			this.AddRecord(
				horizontalPosition,
				yCoordinate,
				blockID);

			return this;
		}

		public MultiBlock AddRecord(
			Position relativePosition,
			int blockID)
		{
			this.AddRecord(
                (byte)relativePosition.X,
                (byte)relativePosition.Z,
                (byte)relativePosition.Y,
				blockID);

			return this;
		}

		public byte[] GetBytes()
		{
			BufferManager bm = new BufferManager();

            bm.WriteInt(this.ChunkX);
            bm.WriteInt(this.ChunkZ);

			bm.WriteVarInt(this.RecordCount);

			for (int i = 0; i < this.RecordCount; i++)
			{
				MultiBlockRecord record = this.Records[i];

				bm.WriteByte(record.HorizontalPosition);
				bm.WriteByte(record.YCoordinate);
				bm.WriteVarInt(record.BlockID);
			}

			return bm.GetBytes();
        }

        #region record class
        public class MultiBlockRecord
		{
			public byte HorizontalPosition { get; private set; }
			public byte YCoordinate { get; private set; }
			public int BlockID { get; private set; }

			public MultiBlockRecord(
				byte horizontalPosition,
				byte yCoordinate,
				int blockID)
			{
				this.HorizontalPosition = horizontalPosition;
				this.YCoordinate = yCoordinate;
				this.BlockID = blockID;
			}
		}
        #endregion
    }
}

