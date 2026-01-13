using SlimeCore.Core.Chunks;
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

        public PacketByteHandler(ServerManager serverManager, ClientHandler clientHandler, QueueHandler queueHandler) 
        {
            this._serverManager = serverManager;
            this._configs = serverManager.Config;
            this._clientHandler = clientHandler;
            this._queueHandler = queueHandler;
        }

        public void HandleBytes(byte packetID, byte[] bytes)
        {
            Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length + "   packet: " + packetID);

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

                    Logger.Log($"{username} trying to join game.");

                    new LoginStartPacket(_clientHandler).Write(username);

                    _clientHandler.State = ClientState.Play;

                    new JoinGamePacket(_clientHandler, _configs).Write();

                    Position spawnPos = new Position(2, 32, 1);

                    new SpawnPositionPacket(_clientHandler).Write(spawnPos);
                    Task.Run(async () => { await _clientHandler.KeepAlive(); });
                    break;
            }
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
                    MinecraftClient mc_client = new MinecraftClient()
                    { 
                        Locale = bm.ReadString(),
                        ViewDistance = bm.ReadByte(),
                        ChatMode = bm.ReadVarInt(),
                        ChatColors = bm.ReadBool(),
                        DisplayedSkinParts = bm.ReadByte(),
                        MainHand = bm.ReadVarInt()
                    };

                    _clientHandler.MC_Client = mc_client;

                    Position spawnPos = new Position(1, 128, 2);
                    new PlayerPositionAndLookPacket(_clientHandler).Write(spawnPos);

                    int areaX = 4;
                    int areaZ = 4;

                    int i = 1;
                    for (int x = 0; x < areaX; x++)
                    {
                        for (int z = 0; z < areaZ; z++)
                        {
                            Chunk chunk = new Chunk();
                            chunk.Build(x, z, true, i);

                            new ChunkDataPacket(_clientHandler).Write(chunk);
                            i++;
                        }
                    }

                    Border border = new Border(new int[2] { 0, 2 })
                    { 
                        Diameter = areaX * 16
                    };

                    //new WorldBorderPacket(_clientHandler).Write(border);

                    break;
                case SB_PlayPacketType.TELEPORT_CONFIRM:
                    
                    break;
                case SB_PlayPacketType.KEEP_ALIVE:
                    /*long keepAliveID = bm.ReadLong();
                    new KeepAlivePacket(_clientHandler).Write(keepAliveID);*/
                    break;
            }
        }
    }
}
