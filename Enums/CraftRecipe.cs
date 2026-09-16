using System;
namespace SlimeCore.Enums
{
	public class CraftRecipe
	{
		public byte WindowID { get; set; }
		public int Recipe { get; set; }
		public bool MakeAll { get; set; }

		public CraftRecipe()
		{

		}

		public CraftRecipe SetWindowID(byte value)
		{
			this.WindowID = value;
			return this;
		}

		public CraftRecipe SetRecipe(int value)
		{
			this.Recipe = value;
			return this;
		}

		public CraftRecipe SetMakeAll(bool value)
		{
			this.MakeAll = value;
			return this;
		}

		public string GetString()
		{
			string result = $"Window ID: {this.WindowID}, Recipe: {this.Recipe}, MakeALL: {this.MakeAll}";
			return result;
		}
	}
}

