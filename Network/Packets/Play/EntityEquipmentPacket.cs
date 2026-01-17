using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class EntityEquipmentPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.ENTITY_EQUIPMENT;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public EntityEquipmentPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            IEntity entity = (IEntity)obj;
            int entity_id = entity.EntityID;

            bm.WriteVarInt(entity_id);
            switch (entity)
            {
                case MinecraftClient:
                    {
                        int hand = (((MinecraftClient)entity).MainHand != 1) ? 1 : 0;
                        int selected_slot = ((MinecraftClient)entity).CurrentSelectedSlot;
                        Slot slot = ((MinecraftClient)entity).Inventory.GetSlotFromID(selected_slot);

                        bm.WriteVarInt(hand);
                        bm.WriteBytes(slot.GetBytes(), false);
                        break;
                    }
            }

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            IEntity entity = (IEntity)obj;
            int entity_id = entity.EntityID;

            bm.WriteVarInt(entity_id);

            switch(entity)
            {
                case MinecraftClient:
                    {
                        int hand = (((MinecraftClient)entity).MainHand != 1) ? 1 : 0;
                        int selected_slot = ((MinecraftClient)entity).CurrentSelectedSlot;
                        Slot slot = ((MinecraftClient)entity).Inventory.GetSlotFromID(selected_slot);

                        bm.WriteVarInt(hand);
                        bm.WriteBytes(slot.GetBytes());
                        break;
                    }
            }


            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
