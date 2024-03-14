using SlimeCore.Core.Metadata;
using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Network.Packets.Queue;
using SlimeCore.Tools.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    internal class SetEntityMetadata : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }
        private bool _broadcast = false;
        private bool _includeSelf = false;
        public SetEntityMetadata(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_ENTITY_METADATA);
        }

        public SetEntityMetadata Broadcast(bool includeSelf)
        {
            _includeSelf = includeSelf;
            _broadcast = true;

            return this;
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            return this;
        }

        public async Task Write() { }

        public async Task Write(Entity entity, Metadata meta)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(entity.EntityID);

            for (int i = 0; i < meta.Meta.Count; i++)
                bm.AddBytes(HandleMetadata(meta.Meta[i].MetaType, meta.Meta[i].MetaValue, meta.Meta[i].MetaObj), false);

            bm.AddByte(0xFF);   //End
            //Console.WriteLine("Sent: {0}", BitConverter.ToString(bm.GetBytes()).Replace("-", " "));
            await QueueHandler.AddPacket(new QueueFactory().SetClientID(ClientHandler.ClientID).SetBytes(bm.GetBytes()).SetBroadcast(_broadcast, _includeSelf).Build());
            //await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public byte[] HandleMetadata(MetadataType type, MetadataValue value, object obj)
        {
            BufferManager bm = new BufferManager();

            byte flag = 0;
            byte index = 0;

            /*switch (entity.EntityType)
            {
                case EntityType.Player:
                    bre
            }*/

            switch (value)
            {
                case MetadataValue.IsStanding:
                    if ((bool)obj)
                        flag = 0x00;
                    break;
                case MetadataValue.IsOnFire:
                    if ((bool)obj)
                        flag = 0x01;
                    break;
                case MetadataValue.IsCrouching:
                    if ((bool)obj)
                        flag = 0x02;
                    break;
                case MetadataValue.IsSprinting:
                    if ((bool)obj)
                        flag = 0x08;
                    break;
                case MetadataValue.IsSwimming:
                    if ((bool)obj)
                        flag = 0x10;
                    break;
                case MetadataValue.IsInvisible:
                    if ((bool)obj)
                        flag = 0x20;
                    break;
                case MetadataValue.HasGlowingEffect:
                    if ((bool)obj)
                        flag = 0x40;
                    break;
                case MetadataValue.IsFlyingWithAnElytra:
                    if ((bool)obj)
                        flag = 0x80;
                    break;

                case MetadataValue.CustomName:

                    break;
            }

            switch (value)
            {
                case MetadataValue.IsStanding:
                case MetadataValue.IsOnFire:
                case MetadataValue.IsCrouching:
                case MetadataValue.IsSprinting:
                case MetadataValue.IsSwimming:
                case MetadataValue.IsInvisible:
                case MetadataValue.HasGlowingEffect:
                case MetadataValue.IsFlyingWithAnElytra:
                    if (type.Equals(MetadataType.Byte))
                        index = 0x00;
                    else if (type.Equals(MetadataType.Pose))
                        index = 0x06;
                    break;

                case MetadataValue.CustomName:
                    index = 0x02;
                    break;
                case MetadataValue.IsCustomNameVisible:
                    index = 0x03;
                    break;
                case MetadataValue.IsSilent:
                    index = 4;
                    break;
                case MetadataValue.Translation:
                    index = 10;
                    break;
                case MetadataValue.Scale:
                    index = 11;
                    break;
                case MetadataValue.Width:
                    index = 19;
                    break;
                case MetadataValue.Height:
                    index = 20;
                    break;
                case MetadataValue.BlockDisplay:
                    index = 22;
                    break;
            }

            bm.AddByte(index);  //Index
            bm.AddByte((byte)type);  //Type
            switch (type)
            {
                case MetadataType.Byte:
                    bm.AddByte((byte)value);
                    break;
                case MetadataType.Pose:
                    switch (value)
                    {
                        case MetadataValue.IsCrouching:
                            bm.AddByte((byte)Pose.SNEAKING);
                            break;
                        case MetadataValue.IsSwimming:
                            bm.AddByte((byte)Pose.SWIMMING);
                            break;
                        default:
                            bm.AddByte((byte)Pose.STANDING);
                            break;
                    }
                    break;
                case MetadataType.Boolean:
                    switch (value)
                    {
                        case MetadataValue.IsCustomNameVisible:
                            bm.AddBool((bool)obj);
                            break;
                        case MetadataValue.HasNoGravity:
                            bm.AddBool((bool)obj);
                            break;
                        case MetadataValue.IsSilent:
                            bm.AddBool((bool)obj);
                            break;
                    }
                    break;
                case MetadataType.VarInt:
                    switch(value)
                    {
                        
                        default:
                            bm.AddVarInt((int)value);
                            break;
                    }
                    break;
                case MetadataType.BlockID:
                    switch (value)
                    {
                        case MetadataValue.BlockDisplay:
                            bm.AddVarInt((int)obj);
                            break;
                        default:
                            bm.AddVarInt((int)value);
                            break;
                    }
                    break;
                case MetadataType.VarLong:
                    bm.AddVarLong((int)value);
                    break;
                case MetadataType.Float:
                    switch (value)
                    {
                        case MetadataValue.Width:
                        case MetadataValue.Height:
                            bm.AddFloat(1);
                            break;
                        default:
                            bm.AddFloat((float)value);
                            break;
                    }
                    break;
                case MetadataType.Vector3:
                    SlimeApi.Position pos = obj as SlimeApi.Position;
                    bm.AddFloat((float)pos.PositionX);
                    bm.AddFloat((float)pos.PositionY);
                    bm.AddFloat((float)pos.PositionZ);
                    /*switch (value)
                    {
                        case MetadataValue.Scale:
                            bm.AddFloat((float)pos.PositionX);
                            bm.AddFloat((float)pos.PositionY);
                            bm.AddFloat((float)pos.PositionZ);
                            break;
                        case MetadataValue.Translation:
                            bm.AddFloat((float)pos.PositionX);
                            bm.AddFloat((float)pos.PositionY);
                            bm.AddFloat((float)pos.PositionZ);
                            break;
                    }*/
                    break;
                case MetadataType.OptChat:
                    bm.AddBool(true);
                    bm.AddString("{\"text\":\"" + (string)obj + "\"}");
                    break;
            }
            //bm.AddByte(player.IsCrouching ? (byte)5 : (byte)0);   //Value

            return bm.GetBytes();
        }

        object IPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }
    }
}
