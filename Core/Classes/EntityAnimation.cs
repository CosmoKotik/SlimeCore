using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Classes
{
    public class EntityAnimation
    {
        public IEntity? Entity { get; set; }
        public int EntityID { get; set; }
        public AnimationType Animation { get; set; }

        public EntityAnimation SetEntity(IEntity entity)
        {
            this.Entity = entity;
            this.EntityID = entity.EntityID;
            return this;
        }
        public EntityAnimation SetAnimation(AnimationType animation) 
        {
            this.Animation = animation;
            return this;
        }
        public EntityAnimation SetEntityID(int entityID) 
        {
            this.EntityID = entityID;
            return this;
        }
        public EntityAnimation GetEntityFromMinecraftClient(MinecraftClient client)
        {
            SetEntity(client);
            return this;
        }
    }
}
