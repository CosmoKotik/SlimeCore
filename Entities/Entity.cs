using SlimeCore.Core.Metadata;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entities
{
    public class Entity
    {
        public int EntityID { get; set; }
        public Guid UUID { get; set; }
        public EntityType EntityType { get; set; }
        public Position CurrentPosition { get; set; }
        public Position PreviousPosition { get; set; }
        public Position Size { get; set; } = new Position(0.7, 1.62, 0.7);
        public bool IsOnGround { get; set; }
        public bool IsCrouching { get; set; }
        public bool IsSwimming { get; set; }
        public bool IsSleeping { get; set; }

        public int AirTicks { get; set; } = 300;
        public string CustomName { get; set; }
        public bool IsCustomNameVisible { get; set; } = false;
        public bool IsSilent { get; set; } = false;
        public bool HasNoGravity { get; set; } = false;

        public int BlockDisplay { get; set; } = 0;

        public Metadata Metadata { get; set; }

        public bool isNpc { get; set; } = false;

        public Player BuildPlayer()
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
                AirTicks = this.AirTicks,
                CustomName = this.CustomName,
                IsCustomNameVisible = this.
                IsSilent = this.IsSilent,
                HasNoGravity = this.HasNoGravity,
                isNpc = this.isNpc
            };
        }

        public Player BuildPlayer(string username)
        {
            return new Player()
            {
                Username = username,
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
                AirTicks = this.AirTicks,
                CustomName = this.CustomName,
                IsCustomNameVisible = this.
                IsSilent = this.IsSilent,
                HasNoGravity = this.HasNoGravity,
                isNpc = this.isNpc
            };
        }
    }
}
