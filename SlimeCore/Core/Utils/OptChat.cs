using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    public class OptChat
    {
        public bool IsPresent = false;
        public ChatMessage OptionalChat = ChatMessage.Empty;

        public static OptChat Empty = new OptChat();
    }
}
