using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chat
{
    public class ChatMessage
    {
        public IEntity Entity { get; set; }
        public ChatPositionType Position { get; set; }
        public string Unformatted_Text { get; set; } = string.Empty;
        public string Json { get; set; } = string.Empty;

        public ChatMessage SetEntity(IEntity entity)
        {
            this.Entity = entity;
            return this;
        }
        public ChatMessage SetPosition(ChatPositionType position)
        { 
            this.Position = position;
            return this;
        }
        public ChatMessage SetText(string text)
        {
            this.Unformatted_Text = text;
            return this;
        }
        public ChatMessage SetJson(string json) 
        {
            this.Json = json;
            return this;
        }

        public string GetJson()
        {
            string text = $"<{this.Entity.Username}> {this.Unformatted_Text}";

            dynamic chat = new
            {
                text = text
            };

            string json = JsonConvert.SerializeObject(chat);
            SetJson(json);
            return this.Json;
        }
    }
}
