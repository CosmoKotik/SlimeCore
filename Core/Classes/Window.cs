using System;
using Newtonsoft.Json.Linq;
using System.Text;
using SlimeCore.Core.Chat;
using SlimeCore.Network;
using SlimeCore.Tools;

namespace SlimeCore.Core.Classes
{
	public class Window : IWindow
	{
        public byte WindowID { get => _windowId; set { } }
        private byte _windowId = 0;

        public string WindowType { get => _windowType; set { } }
        private string _windowType = string.Empty;

        public ChatMessage WindowTitle { get => _windowTitle; set { } }
        private ChatMessage _windowTitle = ChatMessage.Empty();

        public byte NumberOfSlots { get => _numsOfSlots; set { } }
        private byte _numsOfSlots = 0;

        public Window SetWindowID(byte value)
        {
            _windowId = value;
            return this;
        }

        public Window SetWindowType(string value)
        {
            _windowType = value;
            return this;
        }

        public Window SetWindowTitle(string value)
        {
            _windowTitle.SetText(value);
            return this;
        }

        public Window SetNumsOfSlots(byte value)
        {
            _numsOfSlots = value;
            return this;
        }

        public byte[] GetBytes()
        {
            BufferManager bm = new BufferManager();
            
            bm.WriteByte(this.WindowID);
            bm.WriteString(this.WindowType);

            //bm.WriteVarInt(32);
            //byte[] typeBytes = Encoding.UTF8.GetBytes(this.WindowType);
            //for (int i = 0; i < typeBytes.Length; i++)
            //    bm.WriteByte(typeBytes[i]);

            //for (int i = 0; i < 32 - typeBytes.Length; i++)
            //    bm.WriteByte(0x00);

            bm.WriteString(this.WindowTitle.GetJson(false));
            bm.WriteByte(this.NumberOfSlots);

            return bm.GetBytes();
        }
    }
}

