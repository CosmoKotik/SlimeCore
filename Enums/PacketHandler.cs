using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums
{
    public static class PacketHandler
    {
        public static int Get(Versions version, PacketType packet)
        {
            version = Versions.RELEASE_1_20;
            switch (version)
            {
                case Versions.RELEASE_1_19:
                    switch (packet)
                    {
                        //Handshake-Status
                        case PacketType.HANDSHAKE:
                            return 0x00;
                        case PacketType.STATUS:
                            return 0x00;
                        case PacketType.PING:
                            return 0x01;
                        
                        //Login
                        case PacketType.DISCONNECT:
                            return 0x00;
                        case PacketType.ENCRYPTION:
                            return 0x01;
                        case PacketType.LOGIN_SUCCESS:
                            return 0x02;
                        case PacketType.SET_COMPRESSION:
                            return 0x03;
                        case PacketType.LOGIN_PLUGIN_REQUEST:
                            return 0x04;
                        case PacketType.LOGIN_PLUGIN_RESPONSE:
                            return 0x02;

                        //Play
                        case PacketType.LOGIN_PLAY:
                            return 0x23;
                        case PacketType.UPDATE_ENTITY_POSITION:
                            return 0x26;
                        case PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION:
                            return 0x27;
                        case PacketType.UPDATE_ENTITY_ROTATION:
                            return 0x28;
                        case PacketType.PING_PLAY:
                            return 0x2D;
                        case PacketType.SPAWN_PLAYER:
                            return 0x02;
                        case PacketType.SYNCHRONIZE_PLAYER_POSITION:
                            return 0x36;
                    }
                    break;
                case Versions.RELEASE_1_20:
                    switch (packet)
                    {
                        //Handshake-Status
                        case PacketType.HANDSHAKE:
                            return 0x00;
                        case PacketType.STATUS:
                            return 0x00;
                        case PacketType.PING:
                            return 0x01;

                        //Login
                        case PacketType.DISCONNECT:
                            return 0x00;
                        case PacketType.ENCRYPTION:
                            return 0x01;
                        case PacketType.LOGIN_SUCCESS:
                            return 0x02;
                        case PacketType.SET_COMPRESSION:
                            return 0x03;
                        case PacketType.LOGIN_PLUGIN_REQUEST:
                            return 0x04;
                        case PacketType.LOGIN_PLUGIN_RESPONSE:
                            return 0x02;

                        //Play
                        case PacketType.LOGIN_PLAY:
                            return 0x28;
                        case PacketType.UPDATE_ENTITY_POSITION:
                            return 0x2B;
                        case PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION:
                            return 0x2C;
                        case PacketType.UPDATE_ENTITY_ROTATION:
                            return 0x2D;
                        case PacketType.SET_HEAD_ROTATION:
                            return 0x42;
                        case PacketType.PLAYER_COMMAND:
                            return 0x1E;
                        case PacketType.PING_PLAY:
                            return 0x32;
                        case PacketType.SPAWN_PLAYER:
                            return 0x03;
                        case PacketType.SYNCHRONIZE_PLAYER_POSITION:
                            return 0x3C;
                        case PacketType.CHUNK_DATA_AND_UPDATE_LIGHT:
                            return 0x24;
                        case PacketType.SET_CENTER_CHUNK:
                            return 0x4E;
                        case PacketType.SET_PLAYER_POSITION_AND_ROTATION:
                            return 0x15;
                        case PacketType.UNLOAD_CHUNK:
                            return 0x1E;
                        case PacketType.SET_DEFAULT_SPAWN_POSITION:
                            return 0x50;
                        case PacketType.KEEP_ALIVE:
                            return 0x23;
                        case PacketType.PLAYER_INFO_UPDATE:
                            return 0x3A;
                        case PacketType.SET_ENTITY_METADATA:
                            return 0x52;
                        case PacketType.ENTITY_ANIMATION:
                            return 0x04;
                        case PacketType.SET_BLOCK_DESTROY_STAGE:
                            return 0x07;
                        case PacketType.BLOCK_UPDATE:
                            return 0x0A;
                        case PacketType.ACKNOWLEDGE_BLOCK_CHANGE:
                            return 0x06;
                        case PacketType.TELEPORT_ENTITY:
                            return 0x68;
                        case PacketType.SPAWN_ENTITY:
                            return 0x01;
                        case PacketType.REMOVE_ENTITIES:
                            return 0x3E;
                        case PacketType.SET_ENTITY_VELOCITY:
                            return 0x54;
                    }
                    break;
                case Versions.BETA_1_7_3:
                    switch (packet)
                    { 
                        //case PacketType.LOGIN_START
                    }
                    break;
            }

            return 0;
        }

        public static int Get(PacketType packet)
        {
            //Partially only for 759 aka 1.19

            return Get(Versions.RELEASE_1_20, packet);

            switch (packet)
            {
                //Handshake-Status
                case PacketType.HANDSHAKE:
                    return 0x00;
                case PacketType.STATUS:
                    return 0x00;
                case PacketType.PING:
                    return 0x01;

                //Login
                case PacketType.DISCONNECT:
                    return 0x00;
                case PacketType.ENCRYPTION:
                    return 0x01;
                case PacketType.LOGIN_SUCCESS:
                    return 0x02;
                case PacketType.SET_COMPRESSION:
                    return 0x03;
                case PacketType.LOGIN_PLUGIN_REQUEST:
                    return 0x04;
                case PacketType.LOGIN_PLUGIN_RESPONSE:
                    return 0x02;

                //Play
                case PacketType.LOGIN_PLAY:
                    return 0x23;
                case PacketType.UPDATE_ENTITY_POSITION:
                    return 0x26;
                case PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION:
                    return 0x27;
                case PacketType.UPDATE_ENTITY_ROTATION:
                    return 0x28;
                case PacketType.PING_PLAY:
                    return 0x2D;
                case PacketType.SPAWN_PLAYER:
                    return 0x02;
                case PacketType.SYNCHRONIZE_PLAYER_POSITION:
                    return 0x36;
            }

            return 0;
        }

        public static PacketType Get(int packet, ClientState state)
        {
            //Partially only for 759 aka 1.19

            switch (state)
            {
                case ClientState.Handshake:
                    switch (packet)
                    {
                        case 0x00:
                            return PacketType.HANDSHAKE;
                        case 0x01:
                            return PacketType.PING;
                    }
                    break;
                case ClientState.Status:
                    switch (packet)
                    {
                        case 0x00:
                            return PacketType.STATUS;
                        case 0x01:
                            return PacketType.PING;
                    }
                    break;
                case ClientState.Login:
                    switch (packet)
                    {
                        case 0x00:
                            return PacketType.LOGIN_START;
                        case 0x01:
                            return PacketType.ENCRYPTION;
                        case 0x02:
                            return PacketType.LOGIN_SUCCESS;
                        case 0x03:
                            return PacketType.SET_COMPRESSION;
                        /*case 0x04:
                            return PacketType.LOGIN_PLUGIN_REQUEST;
                        case 0x02:
                            return PacketType.LOGIN_PLUGIN_RESPONSE;*/
                    }
                    break;
                case ClientState.Play:
                    switch (packet)
                    {
                        case 0x14:
                            return PacketType.SET_PLAYER_POSITION;
                        case 0x15:
                            return PacketType.SET_PLAYER_POSITION_AND_ROTATION;
                        case 0x16:
                            return PacketType.SET_PLAYER_ROTATION;
                        case 0x23:
                            return PacketType.LOGIN_PLAY;
                        case 0x42:
                            return PacketType.SET_HEAD_ROTATION;
                        case 0x03:
                            return PacketType.SPAWN_PLAYER;
                        case 0x3A:
                            return PacketType.PLAYER_INFO_UPDATE;
                        case 0x36:
                            return PacketType.SYNCHRONIZE_PLAYER_POSITION;
                        case 0x08:
                            return PacketType.CLIENT_INFORMATION;
                        case 0x1E:
                            return PacketType.PLAYER_COMMAND;
                        case 0x2F:
                            return PacketType.SWING_ARM;
                        case 0x1D:
                            return PacketType.PLAYER_ACTION;
                        case 0x31:
                            return PacketType.USE_ITEM_ON;
                        case 0x2B:
                            return PacketType.SET_CREATIVE_MODE_SLOT;
                        case 0x28:
                            return PacketType.SET_HELD_ITEM;
                        case 0x04:
                            return PacketType.CHAT_COMMAND;
                        case 0x05:
                            return PacketType.CHAT_MESSAGE;
                    }
                    break;
            }

            return 0;
        }
    }
}
