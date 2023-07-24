using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using static SlimeCore.Core.Networking.PacketType;

namespace SlimeCore.Core.Networking
{
    public class Listener
    {
        private TcpListener _client = null;
        private NetworkStream _stream;
        private List<byte> _buffer;
        private bool _listening = false;
        private List<ClientHandler> _clientHandlers = new List<ClientHandler>();
        private ServerManager _serverManager;

        public Listener(ServerManager serverManager)
        { 
            _serverManager = serverManager;
        }

        public void Initiate()
        {
            //try
            //{
            //    // Set the TcpListener on port 13000.
            //    Int32 port = 25565;
            //    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            //
            //    // TcpListener server = new TcpListener(port);
            //    _client = new TcpListener(localAddr, port);
            //
            //    // Start listening for client requests.
            //    _client.Start();
            //    
            //    // Buffer for reading data
            //    Byte[] bytes = new Byte[254];
            //    _buffer = new List<byte>();
            //    String data = null;
            //
            //    // Enter the listening loop.
            //
            //    while (true)
            //    {
            //        //Console.Write("Waiting for a connection... ");
            //
            //        // Perform a blocking call to accept requests.
            //        // You could also use server.AcceptSocket() here.
            //        using TcpClient client = _client.AcceptTcpClient();
            //        //Console.WriteLine("Connected!");
            //
            //        data = null;
            //
            //        // Get a stream object for reading and writing
            //        NetworkStream stream = client.GetStream();
            //        _stream = stream;
            //
            //        int i;
            //
            //        // Loop to receive all the data sent by the client.
            //        try
            //        {
            //            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            //            {
            //                // Translate data bytes to a ASCII string.
            //                data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
            //                //Console.WriteLine("Received: {0}", bytes);
            //                Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + i);
            //                //foreach (byte b in bytes)
            //                //    Console.WriteLine("Received: {0}", BitConverter.ToString(bytes));
            //
            //                // Process the data sent by the client.
            //                //data = data.ToUpper();
            //
            //                Console.WriteLine("Packet " + GetPacketId(bytes));
            //                Console.WriteLine("PacketType " + GetPacketType(bytes));
            //
            //                if (GetPacketType(bytes) == PType.Status)
            //                {
            //                    if (GetPacketId(bytes) == 0)
            //                    {
            //                        //string res = "{\r\n    \"version\": {\r\n        \"name\": \"1.12.2\",\r\n        \"protocol\": 340\r\n    },\r\n    \"players\": {\r\n        \"max\": 100,\r\n        \"online\": 0\r\n    },\r\n    \"description\": {\r\n        \"text\": \"Hello world\"\r\n    },\r\n    \"previewsChat\": true,\r\n    \"enforcesSecureChat\": true\r\n}";
            //
            //                        StatusResponse sr = new StatusResponse();
            //                        sr.version = new Response.Version() { name = "1.12.2", protocol = 340 };
            //                        sr.players = new Response.Players() { max = 100, online = 0 };
            //                        sr.description = new Response.Description() { text = "asdasdasasdasd" };
            //                        sr.previewChat = true;
            //                        sr.enforcesSecureChat = true;
            //
            //                        string res = JsonConvert.SerializeObject(sr);
            //
            //                        WriteString(res);
            //                        Flush(0, PType.Status);
            //                    }
            //                    else if (GetPacketId(bytes) == 1)
            //                    {
            //                        // Send back a response.
            //                        stream.Write(bytes, 0, bytes.Length);
            //                        //Console.WriteLine("Sent: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
            //                    }
            //                }
            //                else if (GetPacketType(bytes) == PType.Login)
            //                {
            //                    WriteString("eb2f758d-7722-4fe6-b7e2-06aacab45ffd");
            //                    WriteString("_CosmoKotik_");
            //                    Flush(2, PType.Login);
            //                }
            //
            //                //stream.Write(bytes, 0, bytes.Length);
            //
            //                    //WriteVarInt(0x10);
            //                    //WriteInt128(GuidStringToBigIntPositive("eb2f758d-7722-4fe6-b7e2-06aacab45ffd"));
            //                    //WriteString("eb2f758d-7722-4fe6-b7e2-06aacab45ffd");
            //                    //WriteString(GuidStringToBigIntPositive("eb2f758d-7722-4fe6-b7e2-06aacab45ffd").ToString());
            //                    //WriteUUID(new Guid("eb2f758d-7722-4fe6-b7e2-06aacab45ffd"));
            //                    //WriteVarInt(0);
            //
            //                    /*WriteString("eb2f758d-7722-4fe6-b7e2-06aacab45ffd");
            //                    WriteString("_CosmoKotik_");
            //                    Flush(2);*/
            //
            //                    //Status & Ping
            //                    /*if (GetPacketId(bytes) == 0)
            //                    {
            //                        //string res = "{\r\n    \"version\": {\r\n        \"name\": \"1.12.2\",\r\n        \"protocol\": 340\r\n    },\r\n    \"players\": {\r\n        \"max\": 100,\r\n        \"online\": 0\r\n    },\r\n    \"description\": {\r\n        \"text\": \"Hello world\"\r\n    },\r\n    \"previewsChat\": true,\r\n    \"enforcesSecureChat\": true\r\n}";
            //
            //                        StatusResponse sr = new StatusResponse();
            //                        sr.version = new Response.Version() { name = "1.12.2", protocol = 340 };
            //                        sr.players = new Response.Players() { max = 100, online = 0 };
            //                        sr.description = new Response.Description() { text = "asdasdasasdasd" };
            //                        sr.previewChat = true;
            //                        sr.enforcesSecureChat = true;
            //
            //                        string res = JsonConvert.SerializeObject(sr);
            //
            //                        WriteString(res);
            //                        Flush(0);
            //                    }
            //                    else if (GetPacketId(bytes) == 1)
            //                    {
            //                        // Send back a response.
            //                        stream.Write(bytes, 0, bytes.Length);
            //                        //Console.WriteLine("Sent: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
            //                    }*/
            //                    //Console.WriteLine("Sent: {0}", BitConverter.ToString(msg).Replace("-", " ") + "   " + msg.Length);
            //            }
            //        }
            //        catch (Exception e) { Console.WriteLine(e.ToString()); }
            //    }
            //}
            //catch (SocketException e)
            //{
            //    Console.WriteLine("SocketException: {0}", e);
            //}
            //finally
            //{
            //    _client.Stop();
            //}

            try
            {
                _client = new TcpListener(_serverManager.IP, _serverManager.Port);
                _client.Start();
                _listening = true;

                while (_listening)
                {
                    TcpClient newClient = _client.AcceptTcpClient();
                    ClientHandler ch = new ClientHandler(_serverManager);
                    if (!_clientHandlers.Contains(ch))
                    {
                        _clientHandlers.Add(ch);
                        //_serverManager.ClientHandlers.Add(ch);
                        
                        //new Task(() => { ch.NetworkHandler(_client); }).Start();
                        new Thread(() => { ch.NetworkHandler(newClient, _client); }).Start();
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                _client.Stop();
            }
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

        internal void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteString(string data, int val = -1)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            if (val == - 1)
                WriteVarInt(buffer.Length);
            else
                WriteVarInt(val);
            _buffer.AddRange(buffer);
        }

        internal void WriteHex(string HEX)
        {
            var buffer = StringToByteArrayFastest(HEX);
            WriteVarInt(buffer.Length);
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

            switch (ptype)
            {
                case PType.Status:
                    _stream.Write(buffer, 0, buffer.Length);
                    break;
                case PType.Login:
                    _client.AcceptTcpClient().Client.Send(buffer);
                    break;
            }
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
            return PType.Null;
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
