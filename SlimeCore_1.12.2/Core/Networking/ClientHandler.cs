using Newtonsoft.Json;
using SlimeCore.Core.Entity;
using SlimeCore.Core.Enums;
using SlimeCore.Core.Enums.Protocols;
using SlimeCore.Core.Networking.Packets;
using SlimeCore.Core.Networking.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SlimeCore.Core.Enums.ClientPacketsEnum;
using static SlimeCore.Core.Networking.PacketType;

namespace SlimeCore.Core.Networking
{
    public class ClientHandler
    {
        public Player CurrentPlayer { get; set; }
        public ServerManager ServerManager { get; set; }

        private TcpListener _client = null;
        private NetworkStream _stream;
        private List<byte> _buffer;

        private List<byte> _receivedBuffer;
        private int _lastByte;

        private PType _nextState = PType.Status;

        public ClientHandler()
        {

        }

        public ClientHandler(ServerManager serverManager)
        {
            _buffer = new List<byte>();
            _receivedBuffer = new List<byte>();
            CurrentPlayer = new Player();
            ServerManager = serverManager;
        }

        bool isConnected = false;
        public void NetworkHandler(TcpClient client, TcpListener listener)
        {
            var tcpClient = client;
            var stream = tcpClient.GetStream();
            Byte[] bytes = new Byte[256];

            _stream = stream;
            _client = listener;

            //while (tcpClient.Available > 0)
            while (stream.DataAvailable)
            {
                int i = 0;
                Console.WriteLine("Connected");
                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
                        Console.WriteLine("Type: " + GetPacketType(bytes));
                        _receivedBuffer = bytes.ToList();
                        _lastByte = 2;

                        ChangePacketType(bytes);

                        //IProtocol dsa = new Protocol760();

                        //Console.WriteLine(dsa.HANDSHAKE);

                        //1.19.2
                        /*switch (_nextState)
                        {
                            #region Status
                            case PType.Status:
                                if (GetPacketId(bytes) == 0)
                                {
                                    StatusResponse sr = new StatusResponse();
                                    sr.version = new Response.Version() { name = "SlimeCore - 1.19.2", protocol = 760 };
                                    sr.players = new Response.Players() { max = ServerManager.MaxPlayers, online = 0 };
                                    sr.description = new Response.Description() { text = ServerManager.Motd };
                                    //sr.previewChat = true;
                                    sr.favicon = "";
                                    sr.enforcesSecureChat = true;

                                    string res = JsonConvert.SerializeObject(sr);

                                    Console.WriteLine(res);

                                    WriteString(res);
                                    Flush(0, PType.Null);
                                    //stream.Write(bytes, 0, bytes.Length);

                                    _buffer.Clear();

                                    long asd = ReadLong();
                                    Console.WriteLine(asd);
                                    WriteLong(0);
                                    Flush(0x01, PType.Null);

                                    *//*ServerManager.ClientHandlers.Remove(this);
                                    Console.WriteLine("DisConnected");
                                    tcpClient.Close();
                                    Thread.CurrentThread.Interrupt();*//*
                                }
                                else if (GetPacketId(bytes) == 1)
                                {
                                    // Send back a response.
                                    long asd = ReadLong();
                                    //Console.WriteLine(asd);
                                    //WriteLong(asd);
                                    //Flush(0x01, PType.Status);
                                    //stream.Write(, 0, bytes.Length);

                                    //Console.WriteLine("Sent: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
                                }

                                *//*ServerManager.ClientHandlers.Remove(this);
                                Console.WriteLine("DisConnected");
                                tcpClient.Close();
                                Thread.CurrentThread.Interrupt();*//*
                                break;
                            #endregion
                            #region Login
                            case PType.Login:
                                switch (GetPacketId(bytes))
                                { 
                                    
                                }
                                break;
                            #endregion
                        }
                        if (GetPacketType(bytes) == PType.Login)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                if (!isConnected)
                                {
                                    var UID = new Random().Next(0, 8471);
                                    var username = GetUsername(bytes);
                                    //string username = "_CosmoKotik_";
                                    //string uuid = "eb2f758d-7722-4fe6-b7e2-06aacab45ffd";
                                    var uuid = GetResponseString(username);//
                                    Console.WriteLine(uuid);

                                    CurrentPlayer.Username = username;
                                    CurrentPlayer.Uuid = uuid;
                                    CurrentPlayer.EntityID = UID;
                                    CurrentPlayer.Handler = this;
                                    CurrentPlayer.Gamemode = Gamemodes.Gamemode.Creative;

                                    WriteString(uuid);
                                    WriteString(username);
                                    Flush(0x02, PType.Login);
                                    new JoinGame(this).Write();
                                    new SpawnPosition(this).Write();
                                    new PlayerPositionAndLook(this).Write();
                                    //new SpawnPlayer(this).Write();
                                    ServerManager.AddPlayer(CurrentPlayer, this);

                                    isConnected = true;
                                }
                            }
                            else if (GetPacketId(bytes) == (int)PlayPackets.ClientSettings)
                            {
                                //Console.WriteLine("Reading client settings");
                                new ClientSettings(this).Read(bytes);
                                new ChunkData(this).Write();
                                //Console.WriteLine("Writing PlayerPositionAndLook");
                                //new WorldBorder(this).Write();
                                new PlayerPositionAndLook(this).Write();
                                //new BlockChange(this) { Position = Utils.Vector3.zero, BlockId = 4, Metadata = 0 }.Write();
                                new BlockChange(this) { Position = new Utils.Vector3(0, 18, 0), BlockId = 4, Metadata = 0 }.Write();
                                //new BlockChange(this) { Position = Utils.Vector3.zero, BlockId = 4, Metadata = 0 }.Write();

                                System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(15).TotalMilliseconds);
                                timer.AutoReset = true;
                                timer.Elapsed += (sender, e) => new KeepAlive(this).Write();
                                timer.Start();

                                //new PlayerListItem(this) { Action = 0, Latency = 999 }.Write();
                            }
                            //Disconnect d = new Disconnect(this);
                            //d.Write();
                        }
                        else if (GetPacketType(bytes) == PType.Play)
                        {
                            switch (GetPacketId(bytes))
                            {
                                case (int)PlayPackets.PlayerPositionAndLook:
                                    new PlayerPositionAndLook(this).Read();
                                    new EntityRelativeMove(this).Broadcast(true);
                                    break;
                                case (int)PlayPackets.PlayerPosition:
                                    new PlayerPosition(this).Read();
                                    new EntityRelativeMove(this).Broadcast(true);
                                    break;
                                case (int)PlayPackets.KeepAlive:
                                    long puid = new KeepAlive(this).Read();
                                    Console.WriteLine("KeepAlive received");
                                    break;
                            }

                            //ServerManager.Players.Add(CurrentPlayer);
                        }*/

                        //ONLY FOR 1.12.2 AND ITS BULSHIT AKA CHUNKS FUCK ME SIDEWAYS PLS HELP ME
                        if (GetPacketType(bytes) == PType.Status)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                //string res = "{\r\n    \"version\": {\r\n        \"name\": \"1.12.2\",\r\n        \"protocol\": 340\r\n    },\r\n    \"players\": {\r\n        \"max\": 100,\r\n        \"online\": 0\r\n    },\r\n    \"description\": {\r\n        \"text\": \"Hello world\"\r\n    },\r\n    \"previewsChat\": true,\r\n    \"enforcesSecureChat\": true\r\n}";

                                StatusResponse sr = new StatusResponse();
                                sr.version = new Response.Version() { name = "1.12.2", protocol = 340 };
                                sr.players = new Response.Players() { max = ServerManager.MaxPlayers, online = 0 };
                                sr.description = new Response.Description() { text = ServerManager.Motd };
                                sr.previewChat = true;
                                //sr.enforcesSecureChat = true;

                                string res = JsonConvert.SerializeObject(sr);

                                WriteString(res);
                                Flush(0, PType.Status);
                            }
                            else if (GetPacketId(bytes) == 1)
                            {
                                // Send back a response.
                                stream.Write(bytes, 0, bytes.Length);

                                //Console.WriteLine("Sent: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
                            }

                            /*ServerManager.ClientHandlers.Remove(this);
                            Console.WriteLine("DisConnected");
                            tcpClient.Close();
                            Thread.CurrentThread.Interrupt();*/
                            return;
                        }
                        else if (GetPacketType(bytes) == PType.Login)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                if (!isConnected)
                                {
                                    var UID = new Random().Next(0, 8471);
                                    var username = GetUsername(bytes);
                                    //string username = "_CosmoKotik_";
                                    //string uuid = "eb2f758d-7722-4fe6-b7e2-06aacab45ffd";
                                    var uuid = GetResponseString(username);//
                                    Console.WriteLine(uuid);

                                    CurrentPlayer.Username = username;
                                    CurrentPlayer.Uuid = uuid;
                                    CurrentPlayer.EntityID = UID;
                                    CurrentPlayer.Handler = this;
                                    CurrentPlayer.Gamemode = Gamemodes.Gamemode.Creative;

                                    WriteString(uuid);
                                    WriteString(username);
                                    Flush(0x02, PType.Login);
                                    new JoinGame(this).Write();
                                    new SpawnPosition(this).Write();
                                    new PlayerPositionAndLook(this).Write();
                                    //new SpawnPlayer(this).Write();
                                    ServerManager.AddPlayer(CurrentPlayer, this);

                                    isConnected = true;
                                }
                            }
                            else if (GetPacketId(bytes) == (int)PlayPackets.ClientSettings)
                            {
                                //Console.WriteLine("Reading client settings");
                                new ClientSettings(this).Read(bytes);
                                new ChunkData(this).Write();
                                new Title(this).Write();
                                new DisplayScoreboard(this).Write();
                                new ScoreboardObjective(this).Write();
                                //Console.WriteLine("Writing PlayerPositionAndLook");
                                //new WorldBorder(this).Write();
                                new PlayerPositionAndLook(this).Write();
                                //new BlockChange(this) { Position = Utils.Vector3.zero, BlockId = 4, Metadata = 0 }.Write();
                                new BlockChange(this) { Position = new Utils.Vector3(0, 0, 0), BlockId = 4, Metadata = 0 }.Write();
                                new BlockChange(this) { Position = new Utils.Vector3(0, 0, 1), BlockId = 4, Metadata = 0 }.Write();
                                //new BlockChange(this) { Position = Utils.Vector3.zero, BlockId = 4, Metadata = 0 }.Write();

                                System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(15).TotalMilliseconds);
                                timer.AutoReset = true;
                                timer.Elapsed += (sender, e ) => new KeepAlive(this).Write();
                                timer.Start();

                                //new PlayerListItem(this) { Action = 0, Latency = 999 }.Write();
                            }
                            //Disconnect d = new Disconnect(this);
                            //d.Write();
                        }
                        else if (GetPacketType(bytes) == PType.Play)
                        {
                            switch (GetPacketId(bytes))
                            {
                                case (int)PlayPackets.PlayerPositionAndLook:
                                    new PlayerPositionAndLook(this).Read();
                                    new EntityRelativeMove(this).Broadcast(true);
                                    break;
                                case (int)PlayPackets.PlayerPosition:
                                    new PlayerPosition(this).Read();
                                    new EntityRelativeMove(this).Broadcast(true);
                                    break;
                                case (int)PlayPackets.KeepAlive:
                                    long puid = new KeepAlive(this).Read();
                                    Console.WriteLine("KeepAlive received");
                                    break;
                            }

                            //ServerManager.Players.Add(CurrentPlayer);
                        }

                        _receivedBuffer.Clear();
                        _lastByte = 0;

                    }
                }
                catch (Exception e) 
                { 
                    Console.WriteLine(e.ToString()); 
                }
            }
            //ServerManager.ClientHandlers.Remove(this);
            ServerManager.RemovePlayer(CurrentPlayer, this);
            Console.WriteLine("DisConnected");
            tcpClient.Close();
            Thread.CurrentThread.Interrupt();
            return;
        }

        public static BigInteger GuidStringToBigIntPositive(string guidString)
        {
            Guid g = new Guid(guidString);
            var guidBytes = g.ToByteArray();
            // Pad extra 0x00 byte so value is handled as positive integer
            var positiveGuidBytes = new byte[guidBytes.Length + 1];
            Array.Copy(guidBytes, positiveGuidBytes, guidBytes.Length);

            BigInteger bigInt = new BigInteger(positiveGuidBytes);
            return bigInt;
        }

        internal void WriteUUID(Guid uuid)
        {
            var guid = uuid.ToByteArray();
            var long1 = new byte[8];
            var long2 = new byte[8];
            Array.Copy(guid, 0, long1, 0, 8);
            Array.Copy(guid, 8, long2, 0, 8);
            _buffer.AddRange(long1);
            _buffer.AddRange(long2);
        }

        #region Read
        public int ReadByte()
        {
            var returnData = _receivedBuffer[_lastByte];
            _lastByte++;
            return returnData;
        }
        public int ReadVarInt()
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long. Hehe that's punny.");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }

        public Vector3 ReadPosition()
        {
            _lastByte++;
            int x = Convert.ToInt16(ReadByte());
            _lastByte += 7;
            int y = Convert.ToInt16(ReadByte());
            _lastByte += 7;
            int z = Convert.ToInt16(ReadByte());
            _lastByte += 7;

            return new Vector3(x, y, z);
        }

        public byte[] Read(int length)
        {
            var buffered = new byte[length];
            Array.Copy(_receivedBuffer.ToArray(), _lastByte, buffered, 0, length);
            _lastByte += length;
            return buffered;
        }

        public double ReadDouble()
        {
            var almostValue = Read(8);
            Console.WriteLine(BitConverter.ToDouble(almostValue, 0));
            return BitConverter.ToDouble(almostValue, 0);
        }

        public long ReadLong()
        {
            var almostValue = Read(8);
            return BitConverter.ToInt64(almostValue, 0);
        }

        public float ReadFloat()
        {
            var almost = Read(4);
            var f = BitConverter.ToSingle(almost, 0);
            return f;
        }

        public bool ReadBool()
        {
            var almost = Read(1);
            return BitConverter.ToBoolean(almost);
        }

        public string ReadString()
        {
            var length = ReadVarInt();
            var stringValue = Read(length);

            return Encoding.UTF8.GetString(stringValue);
        }

        #endregion

        #region Write
        internal void WriteVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        internal void WriteVarLong(long value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        public void WriteVector3(Vector3 position)
        {
            var x = Convert.ToInt64(position.X);
            var y = Convert.ToInt64(position.Y);
            var z = Convert.ToInt64(position.Z);
            var toSend = ((x & 0x3FFFFFF) << 38) | ((y & 0xFFF) << 26) | (z & 0x3FFFFFF);
            Console.WriteLine(toSend);
            WriteLong(toSend);
        }
        internal void WriteInt128(BigInteger value)
        {
            _buffer.AddRange(value.ToByteArray());
        }

        internal void WriteInt(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt(uint value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt16(Int16 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt16(UInt16 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt32(Int32 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt32(UInt32 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteInt64(Int64 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteUInt64(UInt64 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteULong(ulong value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteLong(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteBool(bool value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteByte(byte value)
        {
            _buffer.Add(value);
        }
        internal void WriteSByte(sbyte value)
        {
            _buffer.Add((byte)value);
        }
        internal void Write(byte[] value)
        {
            _buffer.AddRange(value);
        }
        internal void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteDouble(double value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteFloat(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteString(string data, int val = -1)
        {
            var buffer = Encoding.UTF8.GetBytes(data).ToList();
            int bufferSize = buffer.Count;

            if (val == -1)
                WriteVarInt(bufferSize);
            else
            {
                WriteVarInt(val);

                if (bufferSize > val)
                    buffer.RemoveRange(val, buffer.Count - val);
                else if (bufferSize < val)
                    for (int i = 0; i < val - bufferSize; i++)
                    {
                        buffer.Add(0);
                    }
            }
            _buffer.AddRange(buffer.ToArray());
        }

        #endregion

        internal void Flush(int id = -1, PType ptype = PType.Null)
        {
            var buffer = _buffer.ToArray();
            _buffer.Clear();
            WriteVarInt(buffer.Length + 1);
            WriteVarInt(id);

            List<byte> buf = new List<byte>(_buffer);
            buf.AddRange(buffer);
            _buffer.Clear();
            buffer = buf.ToArray();
            Console.WriteLine("Sent: {0}", BitConverter.ToString(buffer).Replace("-", " "));
            try { _stream.Write(buffer, 0, buffer.Length); }
            catch { }
            //_client.AcceptTcpClient().Client.Send(buffer);
        }
        private int GetPacketId(byte[] bytes)
        {
            //return Convert.ToInt32(bytes[1]);
            var value = 0;
            var size = 0;
            int b;
            while (((b = bytes[1]) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long. Hehe that's punny.");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }

        private void ChangePacketType(byte[] bytes)
        {
            switch (bytes[bytes.ToList().IndexOf(0xDD) + 1])
            {
                case 1:
                    _nextState = PType.Status;
                    break;
                case 2:
                    _nextState = PType.Login;
                    break;
                default:
                    _nextState = PType.Status;
                    break;
            }
        }

        private PType GetPacketType(byte[] bytes)
        {
            if (bytes.Length > 52)
            {
                if (bytes[bytes.ToList().IndexOf(0xDD) + 1] == -1)
                    return PType.Play;

                switch (bytes[bytes.ToList().IndexOf(0xDD) + 1])
                {
                    case 1:
                        return PType.Status;
                    case 2:
                        return PType.Login;
                    default:
                        return PType.Status;
                }
            }
            return PType.Play;
        }

        private string GetResponseString(string username)
        {
            var httpClient = new HttpClient();

            string url = "https://api.mojang.com/users/profiles/minecraft/" + username;
            var response = httpClient.GetAsync(url).Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(url + "  :    " + contents);
            mcuuid mu = JsonConvert.DeserializeObject<mcuuid>(contents);

            return new Guid(mu.id).ToString();
        }

        public string GetUsername(byte[] bytes)
        {
            string username = "";
            int offset = bytes.ToList().IndexOf(0xDD) + 4;
            if (bytes[0] == 0x02)
                offset = 18;
            byte[] buffer = new byte[bytes[offset] + 1];
            for (int i = offset; i - offset < bytes[offset] + 1; i++)
            {
                buffer[i - offset] = bytes[i];
            }
            for (int i = 0; i < buffer[0]; i++)
            {
                username += Convert.ToChar(buffer[i + 1]);
            }
            return username;
        }
    }
}
