using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Metadata
{
    public enum MetadataType
    {
        Byte,
        VarInt,
        VarLong,
        Float,
        String,
        Chat,
        OptChat,
        Slot,
        Boolean,
        Rotation,
        Position,
        OptPosition,
        Direction,
        OptUUID,
        BlockID,
        OptBlockID,
        NBT,
        Particle,
        VillagerData,
        OptVarInt,
        Pose,
        CatVariant,
        FrogVariant,
        OptGlobalPos,
        PaintingVariant,
        SnifferState,
        Vector3,
        Quaternion
    }
}
