using SlimeCore.Core.Metadata;
using SlimeCore.Entity;
using SlimeCore.Enums;
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

        public SetEntityMetadata(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_ENTITY_METADATA);
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

        public async void Write(Player player, Metadata meta)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddVarInt(player.EntityID);

            for (int i = 0; i < meta.Meta.Count; i++)
                bm.AddBytes(HandleMetadata(meta.Meta[i].MetaType, meta.Meta[i].MetaValue), false);

            bm.AddByte(0xFF);   //End
            await this.ClientHandler.FlushData(bm.GetBytes());
        }

        public byte[] HandleMetadata(MetadataType type, MetadataValue value)
        {
            BufferManager bm = new BufferManager();

            byte flag = 0;
            byte index = 0;
            switch (value)
            {
                case MetadataValue.IsStanding:
                    flag = 0x00;
                    break;
                case MetadataValue.IsOnFire:
                    flag = 0x01;
                    break;
                case MetadataValue.IsCrouching:
                    flag = 0x02;
                    break;
                case MetadataValue.IsSprinting:
                    flag = 0x08;
                    break;
                case MetadataValue.IsSwimming:
                    flag = 0x10;
                    break;
                case MetadataValue.IsInvisible:
                    flag = 0x20;
                    break;
                case MetadataValue.HasGlowingEffect:
                    flag = 0x40;
                    break;
                case MetadataValue.IsFlyingWithAnElytra:
                    flag = 0x80;
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
                case MetadataType.VarInt:
                    bm.AddVarInt((int)value);
                    break;
                case MetadataType.VarLong:
                    bm.AddVarLong((int)value);
                    break;
                case MetadataType.Float:
                    bm.AddFloat((int)value);
                    break;
            }
            //bm.AddByte(player.IsCrouching ? (byte)5 : (byte)0);   //Value

            return bm.GetBytes();
        }
    }
}
