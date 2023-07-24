using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    public class Chat
    {
        public string text = "";
        public bool bold = false;

        public static Chat Empty = new Chat();
    }
}
