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
        private TcpListener _client = null;
        private NetworkStream _stream;
        private List<byte> _buffer;

        private Player _player;
        private ServerManager _serverManager;

        private List<byte> _receivedBuffer;

        public ClientHandler(ServerManager serverManager)
        {
            _buffer = new List<byte>();
            _receivedBuffer = new List<byte>();
            _player = new Player();
            _serverManager = serverManager;
        }

        public void NetworkHandler(TcpListener client)
        {
            var tcpClient = (TcpClient)client.AcceptTcpClient();
            var stream = tcpClient.GetStream();
            Byte[] bytes = new Byte[254];

            _stream = stream;
            _client = client;

            while (tcpClient.Available > 0)
            {
                int i = 0;
                Console.WriteLine("Connected");
                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        _receivedBuffer.AddRange(bytes);
                        Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
                        Console.WriteLine("Type: " + GetPacketType(bytes));
                        if (GetPacketType(bytes) == PType.Status)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                //string res = "{\r\n    \"version\": {\r\n        \"name\": \"1.12.2\",\r\n        \"protocol\": 340\r\n    },\r\n    \"players\": {\r\n        \"max\": 100,\r\n        \"online\": 0\r\n    },\r\n    \"description\": {\r\n        \"text\": \"Hello world\"\r\n    },\r\n    \"previewsChat\": true,\r\n    \"enforcesSecureChat\": true\r\n}";

                                StatusResponse sr = new StatusResponse();
                                sr.version = new Response.Version() { name = "1.12.2", protocol = 340 };
                                sr.players = new Response.Players() { max = 100, online = 0 };
                                sr.description = new Response.Description() { text = "asdasdasasdasd" };
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
                        }
                        else if (GetPacketType(bytes) == PType.Login)
                        {
                            if (GetPacketId(bytes) == 0)
                            {
                                var username = GetUsername(bytes);
                                var uuid = GetResponseString(username);

                                _player.Username = username;
                                _player.Uuid = uuid;

                                WriteString(uuid);
                                WriteString(username);
                                Flush(2, PType.Login);
                                new JoinGame(this).Write();
                            }
                            else if (GetPacketId(bytes) == (int)PlayPackets.ClientSettings)
                            { 
                                
                            }
                            //Disconnect d = new Disconnect(this);
                            //d.Write();
                        }

                        _serverManager.Players.Add(_player);
                        _receivedBuffer.Clear();
                    }
                }
                catch { }
            }
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

        private int _lastByte;
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

        public byte[] Read(int length)
        {
            var buffered = new byte[length];
            Array.Copy(_receivedBuffer.ToArray(), _lastByte, buffered, 0, length);
            _lastByte += length;
            return buffered;
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

        internal void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }
        internal void WriteDouble(Double value)
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
                switch (bytes[16])
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
            int offset = 17;
            byte[] buffer = new byte[bytes[offset] + 1];
            for (int i = offset; i - offset < bytes[offset] + 1; i++)
            {
                buffer[i - offset] = bytes[i];
            }
            for (int i = 0; i < buffer[2]; i++)
            {
                username += Convert.ToChar(buffer[i + 3]);
            }
            return username;
        }
    }
}
