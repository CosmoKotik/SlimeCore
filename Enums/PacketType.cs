﻿using System;
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
        PLAYER_INFO_UPDATE,
        PLAYER_COMMAND,
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
        KEEP_ALIVE
    }
}