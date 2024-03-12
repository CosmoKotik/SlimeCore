﻿using SlimeCore.Entities;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class TeleportEntity : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public TeleportEntity(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.TELEPORT_ENTITY);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async void Write() { }

        public async void Write(Entity entity)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(entity.EntityID);
            bm.AddDouble(entity.CurrentPosition.PositionX);
            bm.AddDouble(entity.CurrentPosition.PositionY);
            bm.AddDouble(entity.CurrentPosition.PositionZ);
            bm.AddByte((byte)entity.CurrentPosition.Yaw);
            bm.AddByte((byte)entity.CurrentPosition.Pitch);
            bm.AddBool(entity.IsOnGround);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}