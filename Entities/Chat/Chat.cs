using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entities.Chat
{
    internal class Chat
    {
        public string insertion { get; set; }
        public ChatClickEvent clickEvent { get; set; }
        public ChatHoverEvent hoverEvent { get; set; }
        public string text { get; set; }

        public string BuildJson()
        { 
            return JsonConvert.SerializeObject(this);
        }
    }

    internal class ChatClickEvent
    { 
        public string action { get; set; }
        public string value { get; set; }
    }

    internal class ChatHoverEvent
    { 
        public string action { set; get; }
        public ChatHoverEventContents contents { get; set; }
    }

    internal class ChatHoverEventContents
    { 
        public string type { get; set; }
        public string id { get; set; }
        public ChatHoverEventContentsName name { get; set; }
    }

    internal class ChatHoverEventContentsName
    {
        public string text { get; set; }
    }
}
