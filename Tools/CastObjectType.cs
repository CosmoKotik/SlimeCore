using SlimeCore.Entity;
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
                    return new SlimeApi.Entity.Player()
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
                    };
                    break;
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
                    break;
            }
            return null;
        }
    }
}
