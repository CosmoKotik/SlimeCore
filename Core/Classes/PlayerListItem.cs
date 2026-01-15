using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class PlayerListItem
    {
        public PlayerListItemAction Action { get; set; }
        public Guid UUID { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Gamemode { get; set; }
        public int Ping { get; set; }
        public bool HasDisplayName { get; set; }
        public string DisplayName { get; set; } = string.Empty;

        public PlayerListItem SetFromMinecraftClient(MinecraftClient client)
        { 
            this.UUID = client.UUID;
            this.Username = client.Username;
            this.Gamemode = client.Gamemode;
            this.Ping = client.Ping;

            return this;
        }
        public PlayerListItem SetAction(PlayerListItemAction action)
        {
            this.Action = action;
            return this;
        }
    }
}
