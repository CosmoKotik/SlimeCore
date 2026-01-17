using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class MinecraftClient : IEntity
    {
        public int EntityID { get; set; } = new Random().Next(int.MaxValue);
        public string Username { get; set; } = string.Empty;
        public Guid UUID { get; set; } = Guid.NewGuid();

        public ClientHandler? ClientHandler { get; set; }
        public Position WorldPosition { get => _worldPosition;
            set
            {
                _previousWorldPosition = _worldPosition;
                _worldPosition = value;
                _chunkPosition = new Position(Math.Ceiling(value.X / 16), Math.Ceiling(value.Y / 16), Math.Ceiling(value.Z / 16));
            }
        }
        private Position _worldPosition;
        public Position PreviousWorldPosition { get => _previousWorldPosition; }
        private Position _previousWorldPosition;
        public Position ChunkPosition { get => _chunkPosition; }
        private Position _chunkPosition;

        public bool IsOnGround { get; set; }

        public int WorldDimension { get; set; }

        public byte Yaw { get; set; }
        public byte Pitch { get; set; }

        public bool IsConnected { get; set; } = true;
        public int Ping { get; set; }

        public Gamemode Gamemode { get; set; } = Gamemode.CREATIVE;
        public Inventory Inventory { get; set; }
        public short CurrentSelectedSlot { get; set; }
        public BlockType CurrentlyHoldingBlock { get; set; }

        public string Locale { get; set; } = string.Empty;          //Language
        public byte ViewDistance { get; set; }                      //Client-side render distance, in chunks
        public int ChatMode { get; set; }                           //0: enabled, 1: commands only, 2: hidden 
        public bool ChatColors { get; set; }                        //cum
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }                           //0: Left, 1: Right

        public void Initialize()
        {
            this.Inventory = new Inventory();
        }

        public MinecraftClient SetLocale(string locale)
        {
            this.Locale = locale;
            return this;
        }
        public MinecraftClient SetViewDistance(byte viewDistance)
        {
            this.ViewDistance = viewDistance;
            return this;
        }
        public MinecraftClient SetChatMode(int chatMode)
        {
            this.ChatMode = chatMode;
            return this;
        }
        public MinecraftClient SetChatColors(bool chatColors)
        {
            this.ChatColors = chatColors;
            return this;
        }
        public MinecraftClient SetDisplayedSkinParts(byte displayedSkinParts)
        {
            this.DisplayedSkinParts = displayedSkinParts;
            return this;
        }
        public MinecraftClient SetMainHand(int mainHand)
        {
            this.MainHand = mainHand;
            return this;
        }

        public MinecraftClient SetWorldPosition(Position pos)
        {
            this.WorldPosition = pos;
            return this;
        }
        public MinecraftClient SetYaw(float yaw)
        {
            int angle = (int)((yaw / 360) * 256);
            this.Yaw = (byte)angle;
            return this;
        }
        public MinecraftClient SetPitch(float pitch)
        {
            this.Pitch = (byte)Math.Clamp(pitch, -90, 90);
            return this;
        }
        public MinecraftClient SetIsOnGround(bool value)
        {
            this.IsOnGround = value;
            return this;
        }
        public MinecraftClient SetCurrentSelectedSlot(short slot)
        {
            this.CurrentSelectedSlot = slot;
            return this;
        }
        public MinecraftClient SetCurrentlyHoldingBlock(BlockType type)
        { 
            this.CurrentlyHoldingBlock = type;
            return this;
        }

        public int GetXZDistance(Position target)
        {
            return (int)Position.GetDistance(_worldPosition.GetXZ(), target.GetXZ());
        }
    }
}
