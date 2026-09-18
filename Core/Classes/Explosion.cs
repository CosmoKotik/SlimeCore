using System;
using SlimeCore.Network;
using SlimeCore.Structs;

namespace SlimeCore.Core.Classes
{
	public class Explosion
	{
		public float X { get; set; } = 0;
		public float Y { get; set; } = 0;
		public float Z { get; set; } = 0;

		public float Radius { get; set; } = 0;
		public int RecordCount { get; set; } = 0;

		public List<byte[]> Records { get; set; }

		public float PlayerMotionX { get; set; } = 0;
		public float PlayerMotionY { get; set; } = 0;
		public float PlayerMotionZ { get; set; } = 0;

        public Explosion()
		{
		}

		public Explosion SetPosition(Position value)
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = value.Z;

			return this;
		}

		public Explosion SetPosX(float value)
		{
			this.X = value;
			return this;
		}

        public Explosion SetPosY(float value)
        {
            this.Y = value;
            return this;
        }

        public Explosion SetPosZ(float value)
        {
            this.Z = value;
            return this;
        }

		public Explosion SetRadius(float value)
		{
			this.Radius = value;
			return this;
		}

		public Explosion SetRecordCount(int value)
		{
			this.RecordCount = value;
			return this;
		}

		public Explosion AddRecord(byte value1, byte value2, byte value3)
		{
			byte[] result = { value1, value2, value3 };
			this.Records.Add(result);

			return this;
		}

		public Explosion SetMotionPosition(Position value)
		{
			this.PlayerMotionX = value.X;
			this.PlayerMotionY = value.Y;
			this.PlayerMotionZ = value.Z;

			return this;
		}

		public Explosion SetMotionX(float value)
		{
			this.PlayerMotionX = value;
			return this;
		}

        public Explosion SetMotionY(float value)
        {
            this.PlayerMotionY = value;
            return this;
        }

        public Explosion SetMotionZ(float value)
        {
            this.PlayerMotionZ = value;
            return this;
        }

        public byte[] GetRecord(int index)
		{
			return this.Records[index];
		}

		public byte[] GetBytes()
		{
			BufferManager bm = new BufferManager();
			bm.WriteFloat(this.X);
            bm.WriteFloat(this.Y);
            bm.WriteFloat(this.Z);
            bm.WriteFloat(this.Radius);

			bm.WriteInt(this.RecordCount);

			//if (this.RecordCount > 0)
			//{
			//	for (int i = 0; i < this.RecordCount; i++)
			//	{
			//		for (int n = 0; n < 3; n++)
			//		{
			//			int index = (i * 3) + n;

			//			bm.WriteByte(this.Records[index]);
			//		}
			//	}
			//}

			bm.WriteFloat(this.PlayerMotionX);
            bm.WriteFloat(this.PlayerMotionY);
            bm.WriteFloat(this.PlayerMotionZ);

			return bm.GetBytes();
        }
    }
}

