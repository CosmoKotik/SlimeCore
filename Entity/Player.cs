using SlimeCore.Core.Metadata;
using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Entity
{
    public class Player
    {
        public int EntityID { get; set; }
        public string Username { get; set; }
        public bool IsHardcore { get; set; }
        public byte Gamemode { get; set; }
        public byte PreviousGamemode { get; set; }
        public int DimensionCount { get; set; }
        public string[] DimensionNames { get; set; }

        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColored { get; set; }
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public bool IsOnGround { get; set; }
        public bool IsCrouching { get; set; }
        public bool IsSwimming { get; set; }
        public bool IsSleeping { get; set; }

        public Metadata Metadata { get; set; }

        public Guid UUID { get; set; }

        public Position CurrentPosition { get; set; }
        public Position PreviousPosition { get; set; }
        public Position Size { get; set; } = new Position(0.7, 1.62, 0.7);

        public Direction LookDirection { get; set; } = Direction.North;
        public Direction HalfDirection { get; set; } = Direction.Top;

        public Player PreviousTickPlayer { get; set; }

        public Inventory Inventory { get; set; }
        public short CurrentHeldItem { get; set; } = 0;

        public Player()
        { 
            CurrentPosition = new Position();
            PreviousPosition = new Position();

            Gamemode = 0x01;
            PreviousGamemode = 0xFF;

            Metadata = new Metadata();
            Metadata.AddMetadata("IsCrouching", MetadataType.Byte, MetadataValue.IsStanding);
            Metadata.AddMetadata("IsCrouchingPose", MetadataType.Pose, MetadataValue.IsStanding);

            Inventory = new Inventory();
            Inventory.Add("crafting_output", 0, 0);
            Inventory.Add("crafting_input", 1, 4);
            Inventory.Add("armor", 5, 8);
            Inventory.Add("main", 9, 35);
            Inventory.Add("hotbar", 36, 44);
            Inventory.Add("offhand", 45, 45);
        }

        public Player Clone()
        {
            return new Player()
            {
                EnableTextFiltering = this.EnableTextFiltering,
                AllowServerListings = this.AllowServerListings,
                ChatColored = this.ChatColored,
                DisplayedSkinParts = this.DisplayedSkinParts,
                MainHand = this.MainHand,
                ChatMode = this.ChatMode,
                UUID = this.UUID,
                CurrentPosition = this.CurrentPosition,
                PreviousPosition = this.PreviousPosition,
                DimensionCount = this.DimensionCount,
                DimensionNames = this.DimensionNames,
                EntityID = this.EntityID,
                Gamemode = this.Gamemode,
                IsCrouching = this.IsCrouching,
                IsHardcore = this.IsHardcore,
                IsOnGround = this.IsOnGround,
                Locale = this.Locale,
                PreviousGamemode = this.PreviousGamemode,
                Username = this.Username,
                ViewDistance = this.ViewDistance,
            };
        }

        public bool CheckIsColliding(Position position)
        {
            Position pos = CurrentPosition.XYZ.Clone();

            double xDecimal = CurrentPosition.PositionX - Math.Truncate(CurrentPosition.PositionX);
            double yDecimal = CurrentPosition.PositionY - Math.Truncate(CurrentPosition.PositionY);
            double zDecimal = CurrentPosition.PositionZ - Math.Truncate(CurrentPosition.PositionZ);

            if (pos.Equals(position, true) || pos.Equals(position - new Position(0, 1, 0), true))
                return true;

            if (xDecimal > 0.700)
                pos.PositionX = Math.Ceiling(pos.PositionX);
            else if (xDecimal < 0.300)
                pos.PositionX = Math.Floor(pos.PositionX - 1);
            else
                pos.PositionX = Math.Floor(pos.PositionX);

            if (zDecimal > 0.700)
                pos.PositionZ = Math.Ceiling(pos.PositionZ);
            else if (zDecimal < 0.300)
                pos.PositionZ = Math.Floor(pos.PositionZ - 1);
            else
                pos.PositionZ = Math.Floor(pos.PositionZ);

            pos.PositionY = Math.Floor(pos.PositionY);

            if (pos.Equals(position))
                return true;

            return false;
        }
    }
}
