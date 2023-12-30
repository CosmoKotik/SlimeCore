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

        public int KickTimeout { get; set; } = 10000;

        private Socket _client;
        //private NetworkStream _stream;
        private ClientState _currentState = ClientState.Handshake;

        private long _lastKeepAliveMiliseconds = 0;

        private Player _player;
        
        private bool _isBeta = false;

        private bool _isAlive = true;
        private bool _isConnected = false;

        private object __lastPacketTimeLock = new object();
        private long _lastPacketTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        private CancellationTokenSource _cancellationToken;

        public ClientHandler(Socket client, ServerManager serverManager)
        {
            this.ServerManager = serverManager;
            this._client = client;
        }

        public async Task NetworkHandler(CancellationTokenSource token)
        {
            this._cancellationToken = token;

            try
            {
                //_stream = _client.GetStream();

                Console.WriteLine("Opened");
                int i = 0;
                byte[] bytes = new byte[1024];
                BufferManager bm = new BufferManager();

                /*_player = new Player()
                { 
                    Username = "_CosmoKotik_",
                    EntityID = ServerManager.Players.Count + 1,
                    UUID = Guid.NewGuid(),
                };*/

                //ServerManager.Players.Add(_player);

                this._client.NoDelay = true;
                this._client.ReceiveTimeout = 1000;
                this._client.SendTimeout = 1000;

                using (this._client)
                {
                    while (_isAlive && this._client.Connected)
                    {
                        if (this._client.Available > 0)
                        {
                            lock(__lastPacketTimeLock)
                                _lastPacketTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                            using (var receivedTask = this._client.ReceiveAsync(bytes, SocketFlags.None))
                            {
                                byte[] correcetdBytes = new byte[receivedTask.Result];
                                Array.Copy(bytes, correcetdBytes, receivedTask.Result);
                                //Console.WriteLine("zalupa: {0}", BitConverter.ToString(correcetdBytes).Replace("-", " ") + "   " + correcetdBytes.Length);
                                bm.SetBytes(correcetdBytes);
                                //Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

                                if (bytes[0].Equals(0x02) && bytes[1].Equals(0x00))
                                    _isBeta = true;

                                int packetSize = bm.GetPacketSize() - 1;
                                int packetID = bm.GetPacketId();

                                /* byte[] buffer = new byte[packetSize];
                                 byte[] bufferRAW = new byte[packetSize + 8];
                                 Array.Copy(bm.GetBytes(), buffer, packetSize);
                                 Array.Copy(bytes, bufferRAW, packetSize + 8);*/

                                byte[] buffer = new byte[packetSize];
                                Array.Copy(bm.GetBytes(), buffer, packetSize);
                                Console.WriteLine("Received: {0}", BitConverter.ToString(buffer).Replace("-", " ") + "   " + buffer.Length);

                                HandleBytes(buffer, correcetdBytes, packetID);

                                //Check if there is more shit in the bytes after the first packet
                                if (bm.GetBytes().Length > packetSize)
                                {
                                    try
                                    {
                                        bm.RemoveRangeByte(packetSize);
                                        //Console.WriteLine("Receiveasdasdasdasdd: {0}", BitConverter.ToString(bm.GetBytes()).Replace("-", " ") + "   " + bm.GetBytes().Length);
                                        packetSize = bm.GetPacketSize() - 1;
                                        packetID = bm.GetPacketId();

                                        buffer = new byte[packetSize];
                                        Array.Copy(bm.GetBytes(), buffer, packetSize);
                                        Console.WriteLine("Received: {0}", BitConverter.ToString(buffer).Replace("-", " ") + "   " + buffer.Length);

                                        HandleBytes(buffer, correcetdBytes, packetID);
                                    }
                                    catch { }
                                }
                            }
                        }

                        if (_lastPacketTime < DateTimeOffset.Now.ToUnixTimeMilliseconds() - KickTimeout)
                            _isAlive = false;

                        await Task.Delay(1);
                    }
                }

                /*while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
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
                                    */
                /*else if (handshake.NextState == (int)ClientState.Login)
                                    {
                                        switch (PacketHandler.Get(packetID, _currentState))
                                        {
                                            case PacketType.LOGIN_START:
                                                _player = new Login(this).Read(buffer) as Player;
                                                _player.EntityID = ServerManager.Players.Count + 1;
                                                ServerManager.Players.Add(_player);
                                                break;
                                        }

                                        new LoginSuccess(this).Write(_player);
                                        new Login(this).Write(_player);

                                        new ChunkDataAndUpdateLight(this).Write();

                                        new SetCenterChunk(this).Write(new Position(0, 0));

                                        new SetDefaultSpawnPosition(this).Write(new Position(5, 0, 5), 0);
                                        new SynchronizePlayerPosition(this).Write(new Position(5, -60, 5));

                                        this._currentState = ClientState.Play;
                                        this._isConnected = true;
                                    }*/
                /*
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
                                    //new LoginSuccess(this).Write();
                                    //new SynchronizePlayerPosition(this).Write();
                                    _player = new Login(this).Read(buffer) as Player;
                                    _player.EntityID = ServerManager.Players.Count + 1;
                                    ServerManager.Players.Add(_player);

                                    new LoginSuccess(this).Write(_player);
                                    new Login(this).Write(_player);

                                    this._currentState = ClientState.Play;
                                    break;
                            }
                            break;
                        case ClientState.Play:
                            //new SynchronizePlayerPosition(this).Write();
                            //FlushData(StringToByteArray("1E 68 CE 03 40 30 5F E3 D1 E0 1E 26 40 3F 07 14 FC E3 66 0E 40 32 40 F3 60 93 5D 30 9F 00 00 04 42 CE 03 6D 09 54 D7 03 00 CE 00 B4 FE EF 0A 2B D7 03 01 62 01 35 FE 30 00"), false);

                            switch (PacketHandler.Get(packetID, _currentState))
                            {
                                case PacketType.CLIENT_INFORMATION:
                                    ClientInformation setting = new ClientInformation(this).Read(buffer) as ClientInformation;

                                    _player.Locale = setting.Locale;
                                    _player.ViewDistance = setting.ViewDistance;
                                    _player.ChatMode = setting.ChatMode;
                                    _player.ChatColored = setting.ChatColored;
                                    _player.DisplayedSkinParts = setting.DisplayedSkinParts;
                                    _player.MainHand = setting.DisplayedSkinParts;
                                    _player.EnableTextFiltering = setting.EnableTextFiltering;
                                    _player.AllowServerListings = setting.AllowServerListings;

                                    new ChunkDataAndUpdateLight(this).Write();

                                    new SetCenterChunk(this).Write(new Position(0, 0));

                                    new SetDefaultSpawnPosition(this).Write(new Position(5, 0, 5), 0);
                                    new SynchronizePlayerPosition(this).Write(new Position(5, -60, 5));

                                    this._currentState = ClientState.Play;
                                    this._isConnected = true;
                                    break;
                                case PacketType.SET_PLAYER_POSITION_AND_ROTATION:
                                    _player.PreviousPosition = _player.CurrentPosition.Clone();

                                    SetPlayerPositionAndRotation playerPosAndRot = new SetPlayerPositionAndRotation(this).Read(buffer) as SetPlayerPositionAndRotation;

                                    _player.CurrentPosition = new Position(playerPosAndRot.X, playerPosAndRot.FeetY, playerPosAndRot.Z, playerPosAndRot.Yaw, playerPosAndRot.Pitch);

                                    Console.WriteLine($"X: {playerPosAndRot.X} Y: {playerPosAndRot.X} Z: {playerPosAndRot.X} Yaw: {playerPosAndRot.Yaw} Pitch: {playerPosAndRot.Pitch}");
                                    break;
                                case PacketType.SET_PLAYER_POSITION:
                                    _player.PreviousPosition = _player.CurrentPosition.Clone();

                                    SetPlayerPosition playerPos = new SetPlayerPosition(this).Read(buffer) as SetPlayerPosition;

                                    _player.CurrentPosition = new Position(playerPos.X, playerPos.FeetY, playerPos.Z);

                                    Console.WriteLine($"X: {playerPos.X} Y: {playerPos.X} Z: {playerPos.X}");
                                    break;
                            }

                            if (_lastKeepAliveMiliseconds + 50000000 < DateTime.UtcNow.Ticks)
                            {
                                new KeepAlive(this).Write();
                                //new SynchronizePlayerPosition(this).Write();
                                _lastKeepAliveMiliseconds = DateTime.UtcNow.Ticks;
                                //new UpdateEntityPositionAndRotation(this).Write();
                            }

                            break;
                    }

                    await Task.Delay(1);
                }*/
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Uh oh: {ex.ToString()}");
            }
            finally
            {
                _isAlive = false;

                if (_client != null)
                {
                    (_client as IDisposable).Dispose();
                    _client = null;
                    Console.WriteLine("Closed");

                    lock(ServerManager.NetClients)
                        ServerManager.NetClients.Remove(this);
                }
            }
            
            token.Cancel();
        }

        private void HandleBytes(byte[] buffer, byte[] bytes, int packetID)
        {
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
                            /*else if (handshake.NextState == (int)ClientState.Login)
                            {
                                switch (PacketHandler.Get(packetID, _currentState))
                                {
                                    case PacketType.LOGIN_START:
                                        _player = new Login(this).Read(buffer) as Player;
                                        _player.EntityID = ServerManager.Players.Count + 1;
                                        ServerManager.Players.Add(_player);
                                        break;
                                }

                                new LoginSuccess(this).Write(_player);
                                new Login(this).Write(_player);

                                new ChunkDataAndUpdateLight(this).Write();

                                new SetCenterChunk(this).Write(new Position(0, 0));

                                new SetDefaultSpawnPosition(this).Write(new Position(5, 0, 5), 0);
                                new SynchronizePlayerPosition(this).Write(new Position(5, -60, 5));

                                this._currentState = ClientState.Play;
                                this._isConnected = true;
                            }*/
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
                            //new LoginSuccess(this).Write();
                            //new SynchronizePlayerPosition(this).Write();
                            _player = new Login(this).Read(buffer) as Player;

                            lock (ServerManager.Players)
                            {
                                _player.EntityID = ServerManager.Players.Count + 1;
                                ServerManager.Players.Add(_player);
                            }

                            new LoginSuccess(this).Write(_player);
                            new Login(this).Write(_player);

                            this._currentState = ClientState.Play;
                            break;
                    }
                    break;
                case ClientState.Play:
                    //new SynchronizePlayerPosition(this).Write();
                    //FlushData(StringToByteArray("1E 68 CE 03 40 30 5F E3 D1 E0 1E 26 40 3F 07 14 FC E3 66 0E 40 32 40 F3 60 93 5D 30 9F 00 00 04 42 CE 03 6D 09 54 D7 03 00 CE 00 B4 FE EF 0A 2B D7 03 01 62 01 35 FE 30 00"), false);

                    switch (PacketHandler.Get(packetID, _currentState))
                    {
                        case PacketType.CLIENT_INFORMATION:
                            ClientInformation setting = new ClientInformation(this).Read(buffer) as ClientInformation;

                            _player.Locale = setting.Locale;
                            _player.ViewDistance = setting.ViewDistance;
                            _player.ChatMode = setting.ChatMode;
                            _player.ChatColored = setting.ChatColored;
                            _player.DisplayedSkinParts = setting.DisplayedSkinParts;
                            _player.MainHand = setting.DisplayedSkinParts;
                            _player.EnableTextFiltering = setting.EnableTextFiltering;
                            _player.AllowServerListings = setting.AllowServerListings;
                            new SynchronizePlayerPosition(this).Write(new Position(5, -60, 5));

                            new ChunkDataAndUpdateLight(this).Write();

                            new SetCenterChunk(this).Write(new Position(0, 0));

                            new SetDefaultSpawnPosition(this).Write(new Position(5, -60, 5), 0);

                            this._currentState = ClientState.Play;
                            OnLoadedWorld().ConfigureAwait(false).GetAwaiter().GetResult();
                            break;
                        case PacketType.SET_PLAYER_POSITION_AND_ROTATION:
                            _player.PreviousPosition = _player.CurrentPosition.Clone();

                            SetPlayerPositionAndRotation playerPosAndRot = new SetPlayerPositionAndRotation(this).Read(buffer) as SetPlayerPositionAndRotation;

                            _player.CurrentPosition = new Position(playerPosAndRot.X, playerPosAndRot.FeetY, playerPosAndRot.Z, playerPosAndRot.Yaw, playerPosAndRot.Pitch);

                            _player.IsOnGround = playerPosAndRot.OnGround;

                            Console.WriteLine($"X: {playerPosAndRot.X} Y: {playerPosAndRot.X} Z: {playerPosAndRot.X} Yaw: {playerPosAndRot.Yaw} Pitch: {playerPosAndRot.Pitch}");
                            break;
                        case PacketType.SET_PLAYER_POSITION:
                            _player.PreviousPosition = _player.CurrentPosition.Clone();

                            SetPlayerPosition playerPos = new SetPlayerPosition(this).Read(buffer) as SetPlayerPosition;

                            _player.CurrentPosition = new Position(playerPos.X, playerPos.FeetY, playerPos.Z);

                            _player.IsOnGround = playerPos.OnGround;

                            Console.WriteLine($"X: {playerPos.X} Y: {playerPos.X} Z: {playerPos.X}");
                            break;
                    }

                    if (_lastKeepAliveMiliseconds + 50000000 < DateTime.UtcNow.Ticks)
                    {
                        new KeepAlive(this).Write();
                        //new SynchronizePlayerPosition(this).Write();
                        _lastKeepAliveMiliseconds = DateTime.UtcNow.Ticks;
                        //new UpdateEntityPositionAndRotation(this).Write();
                    }

                    break;
            }
        }

        public async Task OnLoadedWorld()
        {
            this._isConnected = true;

            //Broadcast to everyone that player joined
            lock (ServerManager.NetClients)
            {
                ServerManager.NetClients.FindAll(x => x != this).ForEach(x =>
                {
                    //_player.PreviousPosition = _player.CurrentPosition.Clone();
                    new PlayerInfoUpdate(x).AddPlayer(_player).Write();
                    new SpawnPlayer(x).Write(_player);

                    new PlayerInfoUpdate(this).AddPlayer(x._player).Write();
                    new SpawnPlayer(this).Write(x._player);
                });
            }

            lock (ServerManager.NetClients)
                ServerManager.NetClients.Add(this);
        }

        public async Task TickUpdate()
        {
            if (!_isAlive || !_isConnected)
                return;

            //Check if player in new chunk
            if ((int)_player.PreviousPosition.PositionZ / 16 != (int)_player.CurrentPosition.PositionZ / 16
                || (int)_player.PreviousPosition.PositionX / 16 != (int)_player.CurrentPosition.PositionX / 16)
            {
                new SetCenterChunk(this).Write(new Position((int)_player.CurrentPosition.PositionX / 16, (int)_player.CurrentPosition.PositionZ / 16));

                Player ueban = new Player() { Username = $"CUM{new Random().Next(273914, 918534)}", UUID = Guid.NewGuid(), EntityID = new Random().Next(), CurrentPosition = new Position(new Random().Next(0, 16), -60, new Random().Next(0, 16)) };

                new PlayerInfoUpdate(this).AddPlayer(ueban).Write();
                new SpawnPlayer(this).Write(ueban);
            }


            //FUN
            if (_player.PreviousPosition.XYZ.PositionX != _player.CurrentPosition.XYZ.PositionX ||
                _player.PreviousPosition.XYZ.PositionY != _player.CurrentPosition.XYZ.PositionY ||
                _player.PreviousPosition.XYZ.PositionZ != _player.CurrentPosition.XYZ.PositionZ)
            {
                /*Player ueban = new Player() { Username = $"GONDON{new Random().Next(273914, 918534)}", UUID = Guid.NewGuid(), EntityID = new Random().Next(), CurrentPosition = _player.CurrentPosition.Clone() };

                new PlayerInfoUpdate(this).AddPlayer(ueban).Write();
                new SpawnPlayer(this).Write(ueban);*/

                /*ServerManager.NetClients.FindAll(x => x != this).ForEach(x =>
                {
                    new UpdateEntityPositionAndRotation(x).Write(_player);
                    Console.WriteLine("asdasdadasdasd");
                });*/
            }

            ServerManager.NetClients.FindAll(x => x != this).ForEach(x =>
            {
                new UpdateEntityPositionAndRotation(x).Write(_player);
            });
        }

        public async Task FlushData(byte[] bytes, bool includeSize = true)
        {
            try
            {
                lock (__lastPacketTimeLock)
                    _lastPacketTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                BufferManager bm = new BufferManager();
                if (includeSize)
                    bm.AddVarInt(bytes.Length);
                //bm.AddByte((byte)bytes.Length);
                bm.InsertBytes(bytes);

                //_stream.Write(bm.GetBytes(), 0, bm.GetBytes().Length);
                await this._client.SendAsync(bm.GetBytes(), SocketFlags.None);
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
