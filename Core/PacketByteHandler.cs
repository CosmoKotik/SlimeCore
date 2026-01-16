using Newtonsoft.Json;
using SlimeCore.Core.Chat;
using SlimeCore.Core.Chunks;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Enums.Serverbound;
using SlimeCore.Network;
using SlimeCore.Network.Packets.Handshake;
using SlimeCore.Network.Packets.Login;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Network.Packets.Status;
using SlimeCore.Network.Queue;
using SlimeCore.Structs;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class PacketByteHandler
    {
        private ServerManager _serverManager;
        private Configs _configs;

        private ClientHandler _clientHandler;
        private QueueHandler _queueHandler;

        private MinecraftClient _minecraftClient;

        public PacketByteHandler(ServerManager serverManager, ClientHandler clientHandler, QueueHandler queueHandler, MinecraftClient minecraftClient)
        {
            this._serverManager = serverManager;
            this._configs = serverManager.Config;
            this._clientHandler = clientHandler;
            this._queueHandler = queueHandler;
            this._minecraftClient = minecraftClient;
        }

        public void HandleBytes(byte packetID, byte[] bytes)
        {
            //Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length + "   packet: " + packetID.ToString("X"));

            switch (_clientHandler.State)
            {
                case ClientState.Handshake:
                    HandleHandshake((HandshakePacketType)packetID, bytes);
                    return;
                case ClientState.Status:
                    HandleStatus((SB_StatusPacketType)packetID, bytes);
                    return;
                case ClientState.Login:
                    HandleLogin((SB_LoginPacketType)packetID, bytes);
                    return;
                case ClientState.Play:
                    HandlePlay((SB_PlayPacketType)packetID, bytes);
                    return;
            }
        }

        private void HandleHandshake(HandshakePacketType packetType, byte[] bytes)
        {
            switch (packetType)
            {
                case HandshakePacketType.HANDSHAKE:
                    HandshakePacket packet = (HandshakePacket)new HandshakePacket().Read(new object[] { bytes });

                    _clientHandler.ClientVersion = (Versions)packet.ProtocolVersion;

                    //new Status(_clientHandler).Write();

                    if (packet.NextState == 1)
                        _clientHandler.State = ClientState.Status;  //Set client state to status mode for further butt fucking
                    else
                        _clientHandler.State = ClientState.Login;   //Set client state to login mode cuz fam wants to join server

                    break;
            }
        }
        private void HandleStatus(SB_StatusPacketType packetType, byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            switch (packetType)
            {
                case SB_StatusPacketType.STATUS_REQUEST:
                    new Status(_clientHandler).Write();
                    break;
                case SB_StatusPacketType.STATUS_PING:
                    new StatusPongPacket(_clientHandler).Write(bm.ReadLong());
                    break;
            }
        }
        private void HandleLogin(SB_LoginPacketType packetType, byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            switch (packetType)
            {
                case SB_LoginPacketType.LOGIN_START:
                    string username = bm.ReadString();
                    _minecraftClient.Username = username;

                    Logger.Log($"{username} trying to join game.");

                    new LoginStartPacket(_clientHandler).Write(_minecraftClient);

                    _clientHandler.State = ClientState.Play;

                    new JoinGamePacket(_clientHandler, _configs).Write(_minecraftClient);

                    Position spawnPos = new Position(2, 32, 1);

                    new SpawnPositionPacket(_clientHandler).Write(spawnPos);
                    Task.Run(async () => { await _clientHandler.KeepAlive(); });
                    break;
            }

            _clientHandler.UpdateMinecraftClient(_minecraftClient);
        }
        private void HandlePlay(SB_PlayPacketType packetType, byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            switch (packetType)
            {
                case SB_PlayPacketType.CLIENT_STATUS:
                    Logger.Warn("Client Status");
                    break;
                case SB_PlayPacketType.CLIENT_SETTINGS:
                    _minecraftClient.SetLocale(bm.ReadString())
                                    .SetViewDistance(bm.ReadByte())
                                    .SetChatMode(bm.ReadVarInt())
                                    .SetChatColors(bm.ReadBool())
                                    .SetDisplayedSkinParts(bm.ReadByte())
                                    .SetMainHand(bm.ReadVarInt());

                    Position spawnPos = new Position(1, 128, 2);

                    _minecraftClient.SetWorldPosition(spawnPos);

                    new PlayerPositionAndLookPacket(_clientHandler).Write(spawnPos);

                    int areaX = 12;
                    int areaZ = 12;

                    int centerOffset = areaX / 2;

                    int i = 1;
                    for (int x = 0 - centerOffset; x < areaX - centerOffset; x++)
                    {
                        for (int z = 0 - centerOffset; z < areaZ - centerOffset; z++)
                        {
                            Chunk chunk = new Chunk();
                            chunk.Build(x, z, true, 2);

                            new ChunkDataPacket(_clientHandler).Write(chunk);
                            i++;
                        }
                    }

                    Border border = new Border(new int[2] { 0, 2 })
                    {
                        Diameter = areaX * 16
                    };

                    _serverManager.AddPlayer(_minecraftClient);

                    PlayerListItem playerListItem = new PlayerListItem().SetAction(PlayerListItemAction.ADD_PLAYER).SetFromMinecraftClient(_minecraftClient);
                    new PlayerListItemPacket(_clientHandler).Broadcast(playerListItem, true);
                    new SpawnPlayerPacket(_clientHandler).Broadcast(_minecraftClient, false);

                    MinecraftClient[] players = _clientHandler.GetAllPlayers();
                    foreach (var player in players)
                    {
                        if (player == _minecraftClient)
                            continue;
                        playerListItem = new PlayerListItem().SetAction(PlayerListItemAction.ADD_PLAYER).SetFromMinecraftClient(player);
                        new PlayerListItemPacket(_clientHandler).Write(playerListItem);
                        new SpawnPlayerPacket(_clientHandler).Write(player);
                    }
                    //new WorldBorderPacket(_clientHandler).Write(border);

                    break;
                case SB_PlayPacketType.TELEPORT_CONFIRM:

                    break;
                case SB_PlayPacketType.KEEP_ALIVE:
                    /*long keepAliveID = bm.ReadLong();
                    new KeepAlivePacket(_clientHandler).Write(keepAliveID);*/
                    break;
                case SB_PlayPacketType.PLAYER:
                    bool player_IsOnGround = bm.ReadBool();
                    _minecraftClient.SetIsOnGround(player_IsOnGround);
                    break;
                case SB_PlayPacketType.PLAYER_POSITION:
                    Position p_pos = new Position(bm.ReadDouble(), bm.ReadDouble(), bm.ReadDouble());
                    bool isOnGround = bm.ReadBool();

                    _minecraftClient.SetWorldPosition(p_pos)
                                    .SetIsOnGround(isOnGround);

                    new EntityRelativeMovePacket(_clientHandler).Broadcast(_minecraftClient, false);

                    /*MinecraftClient test_player = new MinecraftClient().SetWorldPosition(p_pos);
                    PlayerListItem playerListItema = new PlayerListItem().SetAction(PlayerListItemAction.ADD_PLAYER).SetFromMinecraftClient(test_player);
                    
                    new PlayerListItemPacket(_clientHandler).Write(playerListItema);
                    new SpawnPlayerPacket(_clientHandler).Write(test_player);*/

                    //new BlockChangePacket(_clientHandler).Broadcast(new Block().SetPosition(_minecraftClient.WorldPosition).SetBlockType(BlockType.Grass), true);
                    break;
                case SB_PlayPacketType.PLAYER_POSITION_AND_LOOK:
                    Position p_p_pos = new Position(bm.ReadDouble(), bm.ReadDouble(), bm.ReadDouble());
                    float p_l_yaw = bm.ReadFloat();
                    float p_l_pitch = bm.ReadFloat();
                    bool p_p_IsOnGround = bm.ReadBool();

                    _minecraftClient.SetWorldPosition(p_p_pos)
                                    .SetYaw(p_l_yaw)
                                    .SetPitch(p_l_pitch)
                                    .SetIsOnGround(p_p_IsOnGround);

                    new EntityLookAndRelativeMovePacket(_clientHandler).Broadcast(_minecraftClient, false);
                    new EntityHeadLookPacket(_clientHandler).Broadcast(_minecraftClient, false);
                    break;
                case SB_PlayPacketType.PLAYER_LOOK:
                    float yaw = bm.ReadFloat();
                    float pitch = bm.ReadFloat();
                    bool p_l_IsOnGround = bm.ReadBool();

                    _minecraftClient.SetYaw(yaw)
                                    .SetPitch(pitch)
                                    .SetIsOnGround(p_l_IsOnGround);

                    new EntityLookPacket(_clientHandler).Broadcast(_minecraftClient, false);
                    new EntityHeadLookPacket(_clientHandler).Broadcast(_minecraftClient, false);
                    break;
                case SB_PlayPacketType.ANIMATION:
                    HandleAnimation(bm.GetBytes());
                    break;
                case SB_PlayPacketType.CHAT_MESSAGE:
                    HandleChatMessage(bm.GetBytes());
                    break;
                case SB_PlayPacketType.PLAYER_DIGGING:
                    HandlePlayerDigging(bm.GetBytes());
                    break;
            }

            _clientHandler.UpdateMinecraftClient(_minecraftClient);
        }


        public void HandleAnimation(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            bool isOffHand = bm.ReadBool();

            EntityAnimation e_anim = new EntityAnimation().GetEntityFromMinecraftClient(_minecraftClient)
                                                          .SetAnimation(isOffHand ? AnimationType.SWING_OFFHAND : AnimationType.SWING_MAIN_ARM);
            new AnimationPacket(_clientHandler).Broadcast(e_anim, false);
        }

        public void HandleChatMessage(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            string unparsed_text = bm.ReadString();

            ChatMessage chat_msg = new ChatMessage().SetEntity(_minecraftClient)
                                                    .SetText(unparsed_text)
                                                    .SetPosition(ChatPositionType.CHAT_BOX);

            new ChatMessagePacket(_clientHandler).Broadcast(chat_msg, true);
        }

        public void HandlePlayerDigging(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            DiggingStatusType status = (DiggingStatusType)(int)bm.ReadVarInt();
            Position location = bm.ReadPosition();
            Face face = (Face)bm.ReadByte();

            int distance = _minecraftClient.GetXZDistance(location);
            if (distance > 6)
                return;

            switch (status)
            { 
                case DiggingStatusType.STARTED_DIGGING: 
                    {
                        if (_minecraftClient.Gamemode.Equals(Gamemode.CREATIVE))
                        {
                            new BlockChangePacket(_clientHandler).Broadcast(new Block().SetPosition(location).SetBlockType(BlockType.Air), true);
                            Effect break_effect = new Effect()
                                .SetEffectID(EffectType.BLOCK_BREAK)
                                .SetLocation(location)
                                .SetData(1)
                                .SetDisableRelativeVolume(false);

                            new EffectPacket(_clientHandler).Broadcast(break_effect, true);
                        }
                        break;
                    }
                case DiggingStatusType.FINISHED_DIGGING: 
                    {
                        Logger.Warn("Finished digging");
                        break;
                    }
            }
        }
    }
}
