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
                        case PacketType.PING_PLAY:
                            return 0x32;
                        case PacketType.SPAWN_PLAYER:
                            return 0x03;
                        case PacketType.SYNCHRONIZE_PLAYER_POSITION:
                            return 0x3C;
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
                            return PacketType.DISCONNECT;
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
                        case 0x23:
                            return PacketType.LOGIN_PLAY;
                        case 0x26:
                            return PacketType.UPDATE_ENTITY_POSITION;
                        case 0x27:
                            return PacketType.UPDATE_ENTITY_POSITION_AND_ROTATION;
                        case 0x28:
                            return PacketType.UPDATE_ENTITY_ROTATION;
                        case 0x2D:
                            return PacketType.UPDATE_ENTITY_ROTATION;
                        case 0x02:
                            return PacketType.SPAWN_PLAYER;
                        case 0x36:
                            return PacketType.SYNCHRONIZE_PLAYER_POSITION;
                    }
                    break;
            }

            return 0;
        }
    }
}
