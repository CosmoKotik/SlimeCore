﻿using SlimeCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Tools
{
    //CastObjectType
    internal class CastOT
    {
        public static object CastToApi<T>(T obj)
        {
            switch (obj)
            {
                case Player:
                    Player player = obj as Player;
                    return new SlimeApi.Entities.Player()
                    {
                        EnableTextFiltering = player.EnableTextFiltering,
                        AllowServerListings = player.AllowServerListings,
                        ChatColored = player.ChatColored,
                        DisplayedSkinParts = player.DisplayedSkinParts,
                        MainHand = player.MainHand,
                        ChatMode = player.ChatMode,
                        UUID = player.UUID,
                        CurrentPosition = CastToApi(player.CurrentPosition) as SlimeApi.Position,
                        PreviousPosition = CastToApi(player.PreviousPosition) as SlimeApi.Position,
                        DimensionCount = player.DimensionCount,
                        DimensionNames = player.DimensionNames,
                        EntityID = player.EntityID,
                        Gamemode = player.Gamemode,
                        IsCrouching = player.IsCrouching,
                        IsHardcore = player.IsHardcore,
                        IsOnGround = player.IsOnGround,
                        Locale = player.Locale,
                        PreviousGamemode = player.PreviousGamemode,
                        Username = player.Username,
                        ViewDistance = player.ViewDistance,
                        EntityType = (SlimeApi.Enums.EntityType)(player.EntityType),
                        AirTicks = player.AirTicks,
                        BlockDisplay = player.BlockDisplay,
                        CurrentHeldItem = player.CurrentHeldItem,
                        CustomName = player.CustomName,
                        HalfDirection = (SlimeApi.Enums.Direction)player.HalfDirection,
                        HasNoGravity = player.HasNoGravity,
                        Metadata = CastToApi(player.Metadata) as SlimeApi.Metadata,
                        Velocity = CastToApi(player.Velocity) as SlimeApi.Position,
                        isNpc = player.isNpc,
                        IsSilent = player.IsSilent,
                        IsSleeping = player.IsSleeping,
                        IsSwimming = player.IsSwimming,
                        IsCustomNameVisible = player.IsCustomNameVisible,
                        LookDirection = (SlimeApi.Enums.Direction)player.LookDirection
                    };
                case Entities.Entity:
                    Entities.Entity entity = obj as Entities.Entity;
                    return new SlimeApi.Entities.Entity()
                    {
                        UUID = entity.UUID,
                        CurrentPosition = CastToApi(entity.CurrentPosition) as SlimeApi.Position,
                        PreviousPosition = CastToApi(entity.PreviousPosition) as SlimeApi.Position,
                        EntityID = entity.EntityID,
                        IsCrouching = entity.IsCrouching,
                        IsOnGround = entity.IsOnGround,
                        AirTicks = entity.AirTicks,
                        CustomName = entity.CustomName,
                        EntityType = (SlimeApi.Enums.EntityType)entity.EntityType,
                        HasNoGravity = entity.HasNoGravity,
                        IsCustomNameVisible = entity.IsCustomNameVisible,
                        isNpc = entity.isNpc,
                        IsSilent = entity.IsSilent,
                        IsSleeping = entity.IsSleeping,
                        IsSwimming = entity.IsSwimming,
                        BlockDisplay = entity.BlockDisplay,
                        Metadata = CastToApi(entity.Metadata) as SlimeApi.Metadata,
                        Velocity = CastToApi(entity.Velocity) as SlimeApi.Position
                    };
                case Position:
                    Position pos = obj as Position;
                    return new SlimeApi.Position()
                    { 
                        PositionX = pos.PositionX,
                        PositionY = pos.PositionY,
                        PositionZ = pos.PositionZ,
                        Pitch = pos.Pitch,
                        Yaw = pos.Yaw,
                    };
                case Core.Metadata.Metadata:
                    Core.Metadata.Metadata meta = obj as Core.Metadata.Metadata;
                    List<SlimeApi.Metadata> metadatas = new List<SlimeApi.Metadata>();
                    meta.Meta.ForEach(x =>
                    {
                        metadatas.Add(CastToApi(x) as SlimeApi.Metadata);
                    });
                    return new SlimeApi.Metadata()
                    {
                        Meta = metadatas,
                        MetaObj = meta.MetaObj,
                        MetaType = (SlimeApi.MetadataType)meta.MetaType,
                        MetaValue = (SlimeApi.MetadataValue)meta.MetaValue,
                        Name = meta.Name
                    };
            }
            return null;
        }

        public static object CastToCore<T>(T obj)
        {
            switch (obj)
            {
                case SlimeApi.Entities.Player:
                    SlimeApi.Entities.Player player = obj as SlimeApi.Entities.Player;
                    return new Player()
                    {
                        EnableTextFiltering = player.EnableTextFiltering,
                        AllowServerListings = player.AllowServerListings,
                        ChatColored = player.ChatColored,
                        DisplayedSkinParts = player.DisplayedSkinParts,
                        MainHand = player.MainHand,
                        ChatMode = player.ChatMode,
                        UUID = player.UUID,
                        CurrentPosition = CastToCore(player.CurrentPosition) as Position,
                        PreviousPosition = CastToCore(player.PreviousPosition) as Position,
                        DimensionCount = player.DimensionCount,
                        DimensionNames = player.DimensionNames,
                        EntityID = player.EntityID,
                        Gamemode = player.Gamemode,
                        IsCrouching = player.IsCrouching,
                        IsHardcore = player.IsHardcore,
                        IsOnGround = player.IsOnGround,
                        Locale = player.Locale,
                        PreviousGamemode = player.PreviousGamemode,
                        Username = player.Username,
                        ViewDistance = player.ViewDistance,
                        BlockDisplay = player.BlockDisplay,
                    };
                case SlimeApi.Entities.NPC:
                    SlimeApi.Entities.NPC npc = obj as SlimeApi.Entities.NPC;
                    return new NPC()
                    {
                        MainHand = npc.MainHand,
                        UUID = npc.UUID,
                        CurrentPosition = CastToCore(npc.CurrentPosition) as Position,
                        PreviousPosition = CastToCore(npc.PreviousPosition) as Position,
                        EntityID = npc.EntityID,
                        IsCrouching = npc.IsCrouching,
                        IsOnGround = npc.IsOnGround,
                        Username = npc.Username,
                    };
                case SlimeApi.Entities.Entity:
                    SlimeApi.Entities.Entity entity = obj as SlimeApi.Entities.Entity;
                    return new Entity()
                    {
                        UUID = entity.UUID,
                        CurrentPosition = CastToCore(entity.CurrentPosition) as Position,
                        PreviousPosition = CastToCore(entity.PreviousPosition) as Position,
                        EntityID = entity.EntityID,
                        IsCrouching = entity.IsCrouching,
                        IsOnGround = entity.IsOnGround,
                        AirTicks = entity.AirTicks,
                        CustomName = entity.CustomName,
                        EntityType = (Enums.EntityType)entity.EntityType,
                        HasNoGravity = entity.HasNoGravity,
                        IsCustomNameVisible = entity.IsCustomNameVisible,
                        isNpc = entity.isNpc,
                        IsSilent = entity.IsSilent,
                        IsSleeping = entity.IsSleeping,
                        IsSwimming = entity.IsSwimming,
                        BlockDisplay = entity.BlockDisplay,
                        Metadata = CastToCore(entity.Metadata) as Core.Metadata.Metadata,
                        Velocity = CastToCore(entity.Velocity) as Entities.Position
                    };
                case SlimeApi.Position:
                    SlimeApi.Position pos = obj as SlimeApi.Position;
                    return new SlimeCore.Entities.Position()
                    {
                        PositionX = pos.PositionX,
                        PositionY = pos.PositionY,
                        PositionZ = pos.PositionZ,
                        Pitch = pos.Pitch,
                        Yaw = pos.Yaw,
                    };
                case SlimeApi.Metadata:
                    SlimeApi.Metadata meta = obj as SlimeApi.Metadata;
                    List<Core.Metadata.Metadata> metadatas = new List<Core.Metadata.Metadata>();
                    meta.Meta.ForEach(x => 
                    {
                        metadatas.Add(CastToCore(x) as Core.Metadata.Metadata);
                    });
                    return new Core.Metadata.Metadata()
                    {
                        Meta = metadatas,
                        MetaObj = meta.MetaObj,
                        MetaType = (Core.Metadata.MetadataType)meta.MetaType,
                        MetaValue = (Core.Metadata.MetadataValue)meta.MetaValue,
                        Name = meta.Name
                    };
            }
            return null;
        }
    }
}
