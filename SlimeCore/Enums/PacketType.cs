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
        UPDATE_ENTITY_POSITION,
        UPDATE_ENTITY_POSITION_AND_ROTATION,
        UPDATE_ENTITY_ROTATION,
        PING_PLAY,
        SPAWN_PLAYER,
        SYNCHRONIZE_PLAYER_POSITION
    }
}
