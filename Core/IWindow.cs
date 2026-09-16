using System;
using SlimeCore.Core.Chat;

namespace SlimeCore.Core
{
	public interface IWindow
	{
		public byte WindowID { get; set; }
		public string WindowType { get; set; }
		public ChatMessage WindowTitle { get; set; }
		public byte NumberOfSlots { get; set; }
	}
}

