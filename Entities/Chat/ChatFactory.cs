using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entities.Chat
{
    internal class ChatFactory
    {
        public Chat ChatComponent { get; set; }

        public ChatFactory() 
        {
            this.ChatComponent = new Chat();
        }

        public ChatFactory SetText(string text)
        {
            this.ChatComponent.text = text;
            return this;
        }

        public ChatFactory SetInsertion(string text)
        {
            this.ChatComponent.insertion = text;
            return this;
        }

        public ChatFactory SetClickEvent(string action, string value)
        {
            this.ChatComponent.clickEvent = new ChatClickEvent()
            { 
                action = action,
                value = value
            };
            return this;
        }

        public ChatFactory SetHoverEvent(string action, string contentType, string contentId, string contentName)
        {
            this.ChatComponent.hoverEvent = new ChatHoverEvent()
            { 
                action = action,
                contents = new ChatHoverEventContents()
                { 
                    type = contentType,
                    id = contentId,
                    name = new ChatHoverEventContentsName()
                    { 
                        text = contentName
                    }
                }
            };
            return this;
        }

        public Chat Build()
        {
            return this.ChatComponent;
        }
    }
}
