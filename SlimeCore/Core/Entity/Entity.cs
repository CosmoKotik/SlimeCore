using SlimeCore.Core.Enums;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlimeCore.Core.Enums.Pose;

namespace SlimeCore.Core.Entity
{
    public class Entity
    {
        //Index 0
        public bool IsOnFire = false;
        public bool IsCrouching = false;
        public bool Unused = false;
        public bool IsSprinting = false;
        public bool IsSwimming = false;
        public bool IsInvisible = false;
        public bool HasGlowingEffect = false;
        public bool IsFlyingWithElytra = false;

        public int AirTicks = 300;                  //Index 1
        public OptChat CustomName = OptChat.Empty;  //Index 2
        public bool IsCustomNameVisible = false;    //Index 3
        public bool IsSilent = false;               //Index 4
        public bool HasNoGravity = false;           //Index 5
        public Poses Pose = Poses.STANDING;         //Index 6
        public int TicksFrozenInPowderedSnow = 0;   //Index 7

        public byte HandState = 0;                  //Index 8
        public float Health = 1.0f;                  //Index 9
        public int PotionEffectColor = 0;           //Index 10
        public bool IsPotionEffectAmbien = false;
        public int NumberOfArrowsInEntity = 0;
        public int NumberOfBeeStingersInEntity = 0;
        public OptBlockPos BedLocation = OptBlockPos.Empty;
    }
}
