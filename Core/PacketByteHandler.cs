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
            if (!packetID.Equals((int)SB_PlayPacketType.KEEP_ALIVE) &&
                !packetID.Equals((int)SB_PlayPacketType.PLAYER_LOOK) &&
                !packetID.Equals((int)SB_PlayPacketType.PLAYER) &&
                !packetID.Equals((int)SB_PlayPacketType.PLAYER_POSITION) &&
                !packetID.Equals((int)SB_PlayPacketType.PLAYER_POSITION_AND_LOOK) && _clientHandler.State == ClientState.Play)
            {
                Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length + "   packet: " + packetID.ToString("X"));
            }

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

                    Position spawnPos = new Position(48, 65, 47);

                    new SpawnPositionPacket(_clientHandler).Write(spawnPos);

                    _minecraftClient.Initialize();

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

                    Position spawnPos = new Position(0, 65, 0);

                    _minecraftClient.SetWorldPosition(spawnPos);

                    new PlayerPositionAndLookPacket(_clientHandler).Write(spawnPos);

                    int areaX = 16;
                    int areaZ = 16;

                    int centerOffset = areaX / 2;


                    int i = 1;
                    for (int z = 0 - centerOffset; z < areaZ - centerOffset; z++)
                    {
                        for (int x = 0 - centerOffset; x < areaX - centerOffset; x++)
                        {
                            /*Chunk chunk = new Chunk();
                            chunk.Build(x, z, true, 2);*/

                            Chunk chunk = WorldManager.GetChunk(x, z);
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
                case SB_PlayPacketType.CREATIVE_INVENTORY_ACTION:
                    HandleCreativeInventoryAction(bm.GetBytes());
                    break;
                case SB_PlayPacketType.HELD_ITEM_CHANGE:
                    HandleHeldItemChange(bm.GetBytes());
                    break;
                case SB_PlayPacketType.PLAYER_BLOCK_PLACEMENT:
                    HandlePlayerBlockPlacement(bm.GetBytes());
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
                            Block block = new Block().SetPosition(location).SetBlockType(BlockType.Air);

                            new BlockChangePacket(_clientHandler).Broadcast(block, true);
                            Effect break_effect = new Effect()
                                .SetEffectID(EffectType.BLOCK_BREAK)
                                .SetLocation(location)
                                .SetData(1)
                                .SetDisableRelativeVolume(false);

                            new EffectPacket(_clientHandler).Broadcast(break_effect, true);

                            WorldManager.SetBlock(block);
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
        public void HandleCreativeInventoryAction(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            short slot = bm.ReadShort();
            short blockID = bm.ReadShort();
            Slot clickedItem = new Slot()
                .SetSlotID(slot)
                .SetBlockID(blockID);

            if (!blockID.Equals((int)BlockType.Empty_No_Encoding))
                clickedItem.SetItemCount(bm.ReadByte()).SetItemDamage(bm.ReadShort());

            _minecraftClient.Inventory.UpdateSlot(clickedItem);

            if (_minecraftClient.CurrentSelectedSlot.Equals(slot))
                _minecraftClient.SetCurrentlyHoldingBlock(_minecraftClient.Inventory.GetBlockTypeFromSlotID(slot));

            Logger.Warn("Item set");

            /*if (_minecraftClient.CurrentSelectedSlot.Equals(slot))
                new EntityEquipmentPacket(_clientHandler).Broadcast(_minecraftClient, true);*/
        }
        public void HandleHeldItemChange(byte[] bytes) 
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            short hotbarSlotStart = 36;
            short slotOffset = bm.ReadShort();
            short slot_id = (short)(hotbarSlotStart + slotOffset);

            _minecraftClient.SetCurrentSelectedSlot(slot_id);
            _minecraftClient.SetCurrentlyHoldingBlock(_minecraftClient.Inventory.GetBlockTypeFromSlotID(slot_id));
            Logger.Warn($"Slot: {slot_id} Item: {_minecraftClient.Inventory.GetBlockTypeFromSlotID(slot_id)}");

            //new EntityEquipmentPacket(_clientHandler).Broadcast(_minecraftClient, true);
        }
        public void HandlePlayerBlockPlacement(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            Position location = bm.ReadPosition();  //block position
            Face face = (Face)(int)bm.ReadVarInt();
            int hand = bm.ReadVarInt();     //main: 0, off hand: 1
            Position cursor_position = new Position(
                bm.ReadFloat(), 
                bm.ReadFloat(), 
                bm.ReadFloat());

            Position offset = Position.FromFace(face);
            
            BlockType holdingBlock = _minecraftClient.CurrentlyHoldingBlock;
            Console.WriteLine($"Holding rn: {holdingBlock}");

            Block block = new Block().SetPosition(location + offset).SetBlockType(holdingBlock);
            //Console.WriteLine(block.GetChunkPosition().ToString());
            new BlockChangePacket(_clientHandler).Broadcast(block, true);

            WorldManager.SetBlock(block);
        }
    }
}
