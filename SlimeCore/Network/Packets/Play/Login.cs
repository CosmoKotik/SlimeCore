using SlimeCore.Enums;
using SlimeCore.Network.Packets.Nbts;
using SlimeCore.Tools.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeCore.Network.Packets.Play
{
    internal class Login : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public Login(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.LOGIN_PLAY);
        }

        public void Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            throw new NotImplementedException();
        }

        //FUCK MOJANG!!!!!!!!!!!!!!!!
        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            bm.AddByte(15);
            bm.AddBool(false);
            bm.AddByte(1);
            //bm.AddByte(1);

            bm.AddByte(0x7A);
            bm.AddByte(0x00);
            bm.AddByte(0x01);
            bm.AddByte(0xFF);

            #region Dimension
            bm.AddVarInt(3);                          //Dimension size
            bm.AddString("minecraft:overworld");        //Dimensions
            bm.AddString("minecraft:the_end");          //
            bm.AddString("minecraft:the_nether");       //
            #endregion
            #region Registry Codec
            RegistryCodec rc = new RegistryCodec();
            bm.AddBytes(rc.GetRegistryCompound(), false);
            #endregion  //FOR FUCK SAKES

            bm.AddString("minecraft:overworld");     //Dimension type being spawned in
            bm.AddString("minecraft:overworld");     //Dimension being spawned in
            bm.AddLong(-1559193075911489316);    //Random bs abt seed
            bm.AddVarInt(ClientHandler.ServerManager.MaxPlayers);           //Max players
            bm.AddVarInt(ClientHandler.ServerManager.ViewDistance);         //Max players
            bm.AddVarInt(ClientHandler.ServerManager.SimulationDistance);   //Max players
            bm.AddBool(ClientHandler.ServerManager.ReducedDebugInfo);       //Max players
            bm.AddBool(ClientHandler.ServerManager.EnableRespawnScreen);    //Max players
            bm.AddBool(ClientHandler.ServerManager.IsDebug);                //Max players
            bm.AddBool(ClientHandler.ServerManager.IsFlat);                 //Max players
            bm.AddBool(ClientHandler.ServerManager.HasDeathLocation);       //Max players
            bm.AddVarInt(ClientHandler.ServerManager.PortalCooldown);       //Max players

            //bm.AddVarInt(0);

            //Console.WriteLine(BitConverter.ToString(GetRegistryCodec()).Replace("-", " ") + "   " + GetRegistryCodec().Length);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }

        //FUCK ME SIDEWAYS
        private byte[] GetRegistryCodec()
        {
            #region OLD BUT NEED TO FINISH
            BufferManager bm = new BufferManager();

            #region Dimension type
            Nbt dimensionTypeValueElement_PiglinSafe = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "piglin_safe", (byte)0);
            Nbt dimensionTypeValueElement_HasRaids = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "has_raids", (byte)0);
            Nbt dimensionTypeValueElement_MonsterSpawnLightLevel = NbtBuilder.BuildNbt(NbtType.TAG_INT, "monster_spawn_light_level", 0);
            Nbt dimensionTypeValueElement_MonsterSpawnBlockLightLimit = NbtBuilder.BuildNbt(NbtType.TAG_INT, "monster_spawn_block_light_limit", 0);
            Nbt dimensionTypeValueElement_Natural = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "natural", (byte)0);
            Nbt dimensionTypeValueElement_AmbientLight = NbtBuilder.BuildNbt(NbtType.TAG_FLOAT, "ambient_light", 0f);
            Nbt dimensionTypeValueElement_Infiniburn = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "infiniburn", "#minecraft:infiniburn_overworld");
            Nbt dimensionTypeValueElement_RespawnAnchorWorks = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "respawn_anchor_works", (byte)0);
            Nbt dimensionTypeValueElement_HasSkylight = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "has_skylight", (byte)0);
            Nbt dimensionTypeValueElement_BedWorks = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "bed_works", (byte)0);
            Nbt dimensionTypeValueElement_Effects = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "effects", "minecraft:overworld");
            Nbt dimensionTypeValueElement_MinY = NbtBuilder.BuildNbt(NbtType.TAG_INT, "min_y", -64);
            Nbt dimensionTypeValueElement_Height = NbtBuilder.BuildNbt(NbtType.TAG_INT, "height", 256);
            Nbt dimensionTypeValueElement_LogicalHeight = NbtBuilder.BuildNbt(NbtType.TAG_INT, "logical_height", 384);
            Nbt dimensionTypeValueElement_CoordinateScale = NbtBuilder.BuildNbt(NbtType.TAG_DOUBLE, "coordinate_scale", 0.0001);
            Nbt dimensionTypeValueElement_Ultrawarm = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "ultrawarm", (byte)1);
            Nbt dimensionTypeValueElement_HasCeiling = NbtBuilder.BuildNbt(NbtType.TAG_BYTE, "has_ceiling", (byte)0);

            Nbt dimensionTypeValueName = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "name", "minecraft:overworld");
            Nbt dimensionTypeValueId = NbtBuilder.BuildNbt(NbtType.TAG_INT, "id", 0);

            Nbt dimensionTypeValueElement = NbtBuilder.BuildNbtCompound("element", new Nbt[]
              { dimensionTypeValueElement_PiglinSafe,
                dimensionTypeValueElement_HasRaids,
                dimensionTypeValueElement_MonsterSpawnLightLevel,
                dimensionTypeValueElement_MonsterSpawnBlockLightLimit,
                dimensionTypeValueElement_Natural,
                dimensionTypeValueElement_AmbientLight,
                dimensionTypeValueElement_Infiniburn,
                dimensionTypeValueElement_RespawnAnchorWorks,
                dimensionTypeValueElement_HasSkylight,
                dimensionTypeValueElement_BedWorks,
                dimensionTypeValueElement_Effects,
                dimensionTypeValueElement_MinY,
                dimensionTypeValueElement_Height,
                dimensionTypeValueElement_LogicalHeight,
                dimensionTypeValueElement_CoordinateScale,
                dimensionTypeValueElement_Ultrawarm,
                dimensionTypeValueElement_HasCeiling });

            Nbt dimensionTypeType = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "type", "minecraft:dimension_type");
            Nbt dimensionTypeValueCompound = NbtBuilder.BuildNbtCompound("", new Nbt[] { dimensionTypeValueName, dimensionTypeValueId, dimensionTypeValueElement });
            Nbt dimensionTypeValue = NbtBuilder.BuildNbtList("value", new Nbt[] { dimensionTypeValueCompound });

            Nbt dimensionType = NbtBuilder.BuildNbtCompound("minecraft:dimension_type", new Nbt[] { dimensionTypeType, dimensionTypeValue });
            #endregion

            #region Worldgen/Biome
            Nbt biomeRegistryEntryValueElement_Precipitation = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "precipitation", "none");
            Nbt biomeRegistryEntryValueElement_Depth = NbtBuilder.BuildNbt(NbtType.TAG_FLOAT, "depth", 1.5f);
            Nbt biomeRegistryEntryValueElement_Temperature = NbtBuilder.BuildNbt(NbtType.TAG_FLOAT, "temperature", 2.0f);
            Nbt biomeRegistryEntryValueElement_Scale = NbtBuilder.BuildNbt(NbtType.TAG_FLOAT, "scale", 1f);
            Nbt biomeRegistryEntryValueElement_Downfall = NbtBuilder.BuildNbt(NbtType.TAG_FLOAT, "downfall", 0f);
            Nbt biomeRegistryEntryValueElement_Category = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "category", "ocean");
            Nbt biomeRegistryEntryValueElement_Temperature_modifier = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "temperature_modifier", "frozen");

            //cool
            Nbt biomeRegistryEntryValueElement_Effects_SkyColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "sky_color", 8364543);
            Nbt biomeRegistryEntryValueElement_Effects_WaterFogColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "water_fog_color", 8364543);
            Nbt biomeRegistryEntryValueElement_Effects_FogColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "fog_color", 8364543);
            Nbt biomeRegistryEntryValueElement_Effects_WaterColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "water_color", 8364543);
            Nbt biomeRegistryEntryValueElement_Effects_FoliageColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "foliage_color", 8364543);
            Nbt biomeRegistryEntryValueElement_Effects_GrassColor = NbtBuilder.BuildNbt(NbtType.TAG_INT, "grass_color", 8364543);

            Nbt biomeRegistryEntryValueElement_Effects = NbtBuilder.BuildNbtCompound("effects", new Nbt[]
            {
                biomeRegistryEntryValueElement_Effects_SkyColor,
                biomeRegistryEntryValueElement_Effects_WaterFogColor,
                biomeRegistryEntryValueElement_Effects_FogColor,
                biomeRegistryEntryValueElement_Effects_WaterColor,
                biomeRegistryEntryValueElement_Effects_FoliageColor,
                biomeRegistryEntryValueElement_Effects_GrassColor
            });

            Nbt biomeRegistryEntryValueElement = NbtBuilder.BuildNbtCompound("element", new Nbt[]
            {
                biomeRegistryEntryValueElement_Precipitation,
                biomeRegistryEntryValueElement_Depth,
                biomeRegistryEntryValueElement_Temperature,
                biomeRegistryEntryValueElement_Scale,
                biomeRegistryEntryValueElement_Downfall,
                biomeRegistryEntryValueElement_Category,
                biomeRegistryEntryValueElement_Temperature_modifier,
                biomeRegistryEntryValueElement_Effects
            });

            Nbt biomeRegistryEntryValueName = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "name", "minecraft:ocean");
            Nbt biomeRegistryEntryValueId = NbtBuilder.BuildNbt(NbtType.TAG_INT, "id", 0);

            Nbt biomeRegistryEntryType = NbtBuilder.BuildNbt(NbtType.TAG_STRING, "type", "minecraft:chat_type");
            Nbt biomeRegistryEntryValueCompound = NbtBuilder.BuildNbtCompound("", new Nbt[] { biomeRegistryEntryValueName, biomeRegistryEntryValueId, biomeRegistryEntryValueElement });
            Nbt biomeRegistryEntryValue = NbtBuilder.BuildNbtList("value", new Nbt[] { biomeRegistryEntryValueCompound });

            Nbt biomeRegistry = NbtBuilder.BuildNbtCompound("minecraft:worldgen/biome", new Nbt[] { biomeRegistryEntryType, biomeRegistryEntryValue });
            #endregion

            Nbt registryCodec = NbtBuilder.BuildNbtCompound("Registry Codec", new Nbt[] { dimensionType, biomeRegistry });

            //WHAT IS GOING ON
            bm.SetBytes(registryCodec.GetBytes());

            #endregion



            //return bm.GetBytes();

            RegistryCodec rc = new RegistryCodec();

            return rc.GetRegistryCompound();
        }
    }
}
