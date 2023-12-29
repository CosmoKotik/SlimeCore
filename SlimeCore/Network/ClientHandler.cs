using SlimeCore.Core;
using SlimeCore.Entity;
using SlimeCore.Enums;
using SlimeCore.Network.Packets;
using SlimeCore.Network.Packets.Login;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Network.Packets.Status;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    public class ClientHandler
    {
        public ServerManager ServerManager { get; set; }
        public Versions ClientVersion { get; set; }

        private TcpClient _client;
        private NetworkStream _stream;
        private ClientState _currentState = ClientState.Handshake;

        private long _lastKeepAliveMiliseconds = 0;

        private Player _player = new Player();
        
        private bool _isBeta = false;

        public ClientHandler(TcpClient client, ServerManager serverManager)
        {
            this.ServerManager = serverManager;
            this._client = client;
        }

        public async Task NetworkHandler()
        {
            try
            {
                _stream = _client.GetStream();

                Console.WriteLine("Opened");
                int i = 0;
                byte[] bytes = new byte[1024];
                BufferManager bm = new BufferManager();

                while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    //Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

                    bm.SetBytes(bytes);

                    //For beta 1.7.3 purposes
                    if (bytes[0].Equals(0x02) && bytes[1].Equals(0x00))
                        _isBeta = true;

                    int packetSize = bm.GetPacketSize() - 1;
                    int packetID = bm.GetPacketId();

                    byte[] buffer = new byte[packetSize];
                    byte[] bufferRAW = new byte[packetSize + 8];
                    Array.Copy(bm.GetBytes(), buffer, packetSize);
                    Array.Copy(bytes, bufferRAW, packetSize + 8);

                    Console.WriteLine("Received: {0}", BitConverter.ToString(buffer).Replace("-", " ") + "   " + buffer.Length);

                    switch (_currentState)
                    {
                        case ClientState.Handshake:
                            switch (PacketHandler.Get(packetID, _currentState))
                            {
                                case PacketType.HANDSHAKE:
                                    Handshake handshake = (Handshake)new Handshake(this).Read(buffer);
                                    this.ClientVersion = (Versions)handshake.ProtocolVersion;
                                    this._currentState = (ClientState)handshake.NextState;

                                    if (handshake.NextState == (int)ClientState.Status)
                                        new Status(this).Write();
                                    else if (handshake.NextState == (int)ClientState.Login)
                                    {
                                        new LoginSuccess(this).Write();
                                        new Login(this).Write();

                                        new ChunkDataAndUpdateLight(this).Write();

                                        new SetCenterChunk(this).Write();

                                        new SetDefaultSpawnPosition(this).Write(new Position(5, 0, 5), 0);
                                        new SynchronizePlayerPosition(this).Write(new Position(5, -60, 5));

                                        this._currentState = ClientState.Play;
                                    }
                                    break;
                                case PacketType.PING:
                                    new PingLegacy(this).Write();
                                    break;
                            }
                            break;
                        case ClientState.Status:
                            switch (PacketHandler.Get(packetID, _currentState))
                            {
                                case PacketType.STATUS:
                                    new Status(this).Write();
                                    break;
                                case PacketType.PING:
                                    new Ping(this).ReadWrite(bytes);
                                    break;
                            }
                            break;
                        case ClientState.Login:
                            switch (PacketHandler.Get(packetID, _currentState))
                            {
                                case PacketType.LOGIN_START:
                                    new LoginSuccess(this).Write();
                                    //new SynchronizePlayerPosition(this).Write();

                                    break;
                            }
                            break;
                        case ClientState.Play:
                            //new SynchronizePlayerPosition(this).Write();
                            //FlushData(StringToByteArray("1E 68 CE 03 40 30 5F E3 D1 E0 1E 26 40 3F 07 14 FC E3 66 0E 40 32 40 F3 60 93 5D 30 9F 00 00 04 42 CE 03 6D 09 54 D7 03 00 CE 00 B4 FE EF 0A 2B D7 03 01 62 01 35 FE 30 00"), false);
                            if (_lastKeepAliveMiliseconds + 50000000 < DateTime.UtcNow.Ticks)
                            {
                                new KeepAlive(this).Write();
                                //new SynchronizePlayerPosition(this).Write();
                                _lastKeepAliveMiliseconds = DateTime.UtcNow.Ticks;
                                //new UpdateEntityPositionAndRotation(this).Write();
                            }

                            switch (PacketHandler.Get(packetID, _currentState))
                            {
                                case PacketType.SET_PLAYER_POSITION_AND_ROTATION:
                                    SetPlayerPositionAndRotation playerPosAndRot = new SetPlayerPositionAndRotation(this).Read(buffer) as SetPlayerPositionAndRotation;

                                    _player.CurrentPosition = new Position(playerPosAndRot.X, playerPosAndRot.FeetY, playerPosAndRot.Z, playerPosAndRot.Yaw, playerPosAndRot.Pitch);

                                    Console.WriteLine($"X: {playerPosAndRot.X} Y: {playerPosAndRot.X} Z: {playerPosAndRot.X} Yaw: {playerPosAndRot.Yaw} Pitch: {playerPosAndRot.Pitch}");

                                    _player.PreviousPosition = _player.CurrentPosition.Clone();
                                    break;
                                case PacketType.SET_PLAYER_POSITION:
                                    SetPlayerPosition playerPos = new SetPlayerPosition(this).Read(buffer) as SetPlayerPosition;

                                    _player.CurrentPosition = new Position(playerPos.X, playerPos.FeetY, playerPos.Z);

                                    Console.WriteLine($"X: {playerPos.X} Y: {playerPos.X} Z: {playerPos.X}");

                                    _player.PreviousPosition = _player.CurrentPosition.Clone();
                                    break;
                            }

                            break;
                    }

                    await Task.Delay(1);
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Uh oh: {ex.ToString()}");
            }
            finally
            {
                if (_client != null)
                {
                    (_client as IDisposable).Dispose();
                    _client = null;
                    Console.WriteLine("Closed");
                }
            }
        }

        public async Task FlushData(byte[] bytes, bool includeSize = true)
        {
            try
            {
                BufferManager bm = new BufferManager();
                if (includeSize)
                    bm.AddVarInt(bytes.Length);
                //bm.AddByte((byte)bytes.Length);
                bm.InsertBytes(bytes);

                _stream.Write(bm.GetBytes(), 0, bm.GetBytes().Length);
                //stream.Flush();
                Console.WriteLine("Sent: {0}", BitConverter.ToString(bm.GetBytes()).Replace("-", " "));
            }
            catch
            { 
                
            }
        }
        private static byte[] StringToByteArray(string hexc)
        {
            string[] hexValuesSplit = hexc.Split(' ');
            byte[] bytes = new byte[hexValuesSplit.Length];
            int i = 0;
            foreach (string hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}",
                //                    hex, value, stringValue, charValue);
                bytes[i] = (byte)value;
                i++;
            }

            return bytes;
        }
        private int GetStaticPacketId(PacketType type)
        {
            return PacketHandler.Get(type);
        }

        private int GetStaticPacketId(Versions version, PacketType type)
        {
            return PacketHandler.Get(version, type);
        }
    }
}
