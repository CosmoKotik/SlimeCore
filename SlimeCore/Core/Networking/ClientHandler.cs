using Newtonsoft.Json;
using SlimeCore.Core.Entity;
using SlimeCore.Core.Enums;
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
                                sr.enforcesSecureChat = true;

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

                            ServerManager.ClientHandlers.Remove(this);
                            Console.WriteLine("DisConnected");
                            tcpClient.Close();
                            Thread.CurrentThread.Interrupt();
                            return;
                        }
                        else if (GetPacketType(bytes) == PType.Login)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                var UID = new Random().Next(0, 8471);
                                var username = GetUsername(bytes);
                                var uuid = GetResponseString(username);

                                CurrentPlayer.Username = username;
                                CurrentPlayer.Uuid = uuid;
                                CurrentPlayer.EntityID = UID;
                                CurrentPlayer.Handler = this;
                                CurrentPlayer.Gamemode = Gamemodes.Gamemode.Creative;

                                WriteString(uuid);
                                WriteString(username);
                                Flush(2, PType.Login);
                                new JoinGame(this).Write();
                                new SpawnPosition(this).Write();
                                //new SpawnPlayer(this).Write();
                                ServerManager.AddPlayer(CurrentPlayer, this);
                            }
                            else if (GetPacketId(bytes) == (int)PlayPackets.ClientSettings)
                            {
                                new ClientSettings(this).Read(bytes);
                                new ChunkData(this).Write();
                                new PlayerPositionAndLook(this).Write();
                                new BlockChange(this) { Position = new Vector3(0, 1, 0), BlockId = 1, Metadata = 0 }.Write();
                                new BlockChange(this) { Position = Vector3.Zero, BlockId = 2, Metadata = 0 }.Write();
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
                            }

                            //ServerManager.Players.Add(CurrentPlayer);
                        }

                        _receivedBuffer.Clear();
                        _lastByte = 0;

                    }
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
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

        internal void WriteVarInt(int value)
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
        internal void Write(byte[] value)
        {
            _buffer.AddRange(value);
        }
        internal void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteDouble(Double value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteFloat(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteString(string data, int val = -1)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            if (val == -1)
                WriteVarInt(buffer.Length);
            else
                WriteVarInt(val);
            _buffer.AddRange(buffer);
        }

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
            _stream.Write(buffer, 0, buffer.Length);
            //_client.AcceptTcpClient().Client.Send(buffer);
        }
        private int GetPacketId(byte[] bytes)
        {
            return Convert.ToInt32(bytes[1]);
        }

        private PType GetPacketType(byte[] bytes)
        {
            if (bytes.Length > 16)
            {
                switch (bytes[bytes.ToList().IndexOf(0xDD) + 1])
                {
                    case 1:
                        return PType.Status;
                    case 2:
                        return PType.Login;
                }
            }
            return PType.Play;
        }

        private string GetResponseString(string username)
        {
            var httpClient = new HttpClient();

            var response = httpClient.GetAsync("https://api.mojang.com/users/profiles/minecraft/" + username).Result;
            var contents = response.Content.ReadAsStringAsync().Result;

            mcuuid mu = JsonConvert.DeserializeObject<mcuuid>(contents);

            return new Guid(mu.id).ToString();
        }

        public string GetUsername(byte[] bytes)
        {
            string username = "";
            int offset = bytes.ToList().IndexOf(0xDD) + 4;
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
