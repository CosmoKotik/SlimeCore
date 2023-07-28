using SlimeCore.Enums;
using SlimeCore.Network.Packets;
using SlimeCore.Network.Packets.Login;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Network.Packets.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    public class ClientHandler
    {
        public Versions ClientVersion { get; set; }

        private TcpClient _client;
        private NetworkStream _stream;
        private ClientState _currentState = ClientState.Handshake;

        public ClientHandler(TcpClient client)
        {
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
                    Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

                    bm.SetBytes(bytes);

                    int packetSize = bm.GetPacketSize() - 1;
                    int packetID = bm.GetPacketId();

                    byte[] buffer = new byte[packetSize];
                    for (int x = 0; x < packetSize; x++)
                    {
                        buffer[x] = bm.GetBytes()[x];
                    }
                    //bytes = new byte[packetSize];
                    //bytes = (byte[])buffer.Clone();

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
                                        new SynchronizePlayerPosition(this).Write();
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
                                    new SynchronizePlayerPosition(this).Write();
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
