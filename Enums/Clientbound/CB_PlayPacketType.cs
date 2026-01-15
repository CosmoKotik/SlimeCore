using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums.Clientbound
{
    public enum CB_PlayPacketType
    {
        JOIN_GAME = 0x23,
        SPAWN_POSITION = 0x46,
        PLAYER_POSITION_AND_LOOK = 0x2F,
        CHUNK_DATA = 0x20,
        KEEP_ALIVE = 0x1F,
        WORLD_BORDER = 0x38,
        SPAWN_PLAYER = 0x05,
        PLAYER_LIST_ITEM = 0x2E,
        ENTITY_RELATIVE_MOVE = 0x26,
        ENTITY_LOOK_AND_RELATIVE_MOVE = 0x27,
        ENTITY_LOOK = 0x28,
        ENTITY_HEAD_LOOK = 0x36,
        DESTROY_ENTITIES = 0x32
    }
}
