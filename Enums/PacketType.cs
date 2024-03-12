using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums
{
    public enum PacketType
    { 
        //Handshake
        HANDSHAKE,

        //Status-Ping
        STATUS,
        PING,

        //Login
        DISCONNECT,
        LOGIN_START,
        ENCRYPTION,
        SET_COMPRESSION,
        LOGIN_PLUGIN_REQUEST,
        LOGIN_PLUGIN_RESPONSE,
        LOGIN_SUCCESS,
        
        //Play
        LOGIN_PLAY,
        CLIENT_INFORMATION,
        SET_PLAYER_POSITION_AND_ROTATION,
        SET_PLAYER_POSITION,
        SET_PLAYER_ROTATION,
        SET_BLOCK_DESTROY_STAGE,
        BLOCK_UPDATE,
        ACKNOWLEDGE_BLOCK_CHANGE,
        PLAYER_INFO_UPDATE,
        PLAYER_COMMAND,
        USE_ITEM_ON,
        SET_ENTITY_METADATA,
        ENTITY_ANIMATION,
        SWING_ARM,
        UPDATE_ENTITY_POSITION,
        UPDATE_ENTITY_POSITION_AND_ROTATION,
        UPDATE_ENTITY_ROTATION,
        SET_HEAD_ROTATION,
        PING_PLAY,
        SPAWN_PLAYER,
        SYNCHRONIZE_PLAYER_POSITION,
        CHUNK_DATA_AND_UPDATE_LIGHT,
        UNLOAD_CHUNK,
        SET_CENTER_CHUNK,
        SET_DEFAULT_SPAWN_POSITION,
        KEEP_ALIVE,
        PLAYER_ACTION,
        SET_CREATIVE_MODE_SLOT,
        SET_HELD_ITEM,
        TELEPORT_ENTITY,
        SPAWN_ENTITY,
        REMOVE_ENTITIES,
        SET_ENTITY_VELOCITY,
        CHAT_COMMAND,
        CHAT_MESSAGE
    }
}
