using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entities
{
    public class NPC : Entity
    {
        public string Username { get; set; }
        public short CurrentHeldItem { get; set; } = 0;
        public int MainHand { get; set; }

        public NPC()
        {
            isNpc = true;
        }

        public new Player BuildPlayer()
        {
            return new Player()
            {
                EntityID = this.EntityID,
                UUID = this.UUID,
                EntityType = this.EntityType,
                CurrentPosition = this.CurrentPosition,
                PreviousPosition = this.PreviousPosition,
                Size = this.Size,
                IsOnGround = this.IsOnGround,
                IsCrouching = this.IsCrouching,
                IsSwimming = this.IsSwimming,
                IsSleeping = this.IsSleeping,
                Username = this.Username,
                CurrentHeldItem = this.CurrentHeldItem,
                MainHand = this.MainHand,
                isNpc = true
            };
        }
    }
}
