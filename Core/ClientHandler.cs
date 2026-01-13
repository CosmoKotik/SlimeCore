using SlimeCore.Enums;
using SlimeCore.Network;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class ClientHandler
    {
        public Socket Client { get; set; }
        private object _clientLock = new object();

        public Versions ClientVersion { get; set; }

        public bool IsDisposed { get => _isDisposed; }
        private bool _isDisposed = false;

        public bool IsAlive { get => _isAlive; }
        private bool _isAlive = true;

        public ServerManager ServerManager;

        public QueueHandler QueueHandler;

        public MinecraftClient MC_Client;

        public ClientState State { get; set; }

        public ClientHandler(ServerManager serverManager)
        {
            this.ServerManager = serverManager;
            this.QueueHandler = new QueueHandler(this);
        }

        public async Task HandleClient(Socket client)
        {
            this.Client = client;

            using (this.Client)
            {
                this.Client.NoDelay = true;

                byte[] buffer = new byte[2097151];
                BufferManager bm = new BufferManager();

                while (IsAlive && !IsDisposed)
                {
                    using (var received = this.Client.ReceiveAsync(buffer, SocketFlags.None))
                    {
                        if (received.Result == 0)
                        {
                            Logger.Error("Player has lost connection with the server.");
                            this.Dispose();
                        }

                        byte[] bytes = new byte[received.Result];
                        Array.Copy(buffer, bytes, received.Result);
                        Console.WriteLine("Received==: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

                        bm.SetBytes(bytes);

                        while (bm.GetBytes().Length > 0)
                        {
                            int packetSize = bm.GetPacketSize();
                            int packetID = bm.GetPacketId();

                            byte[] packetBytes = new byte[packetSize];
                            Array.Copy(bm.GetBytes(), packetBytes, packetSize);
                            new PacketByteHandler(ServerManager, this, QueueHandler).HandleBytes((byte)packetID, packetBytes);

                            bm.RemoveRangeByte(packetSize);
                        }
                    }
                    await Task.Delay(1);
                }

                Console.WriteLine("end");
            }
        }

        public async Task KeepAlive()
        {
            while (IsAlive && !IsDisposed)
            {
                long KeepAliveID = new Random().NextInt64(long.MaxValue);
                new KeepAlivePacket(this).Write(KeepAliveID);

                await Task.Delay(5000);
            }
        }

        public async void SendAsync(byte[] bytes)
        {
            if (_isDisposed)
            {
                Logger.Warn("Disposed", true);
                return;
            }

            BufferManager bm = new BufferManager();

            bm.InsertBytes(bytes);

            Socket? client;

            lock (_clientLock)
                client = this.Client;

            //Console.WriteLine("Sent: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

            if (client.Connected)
                await client.SendAsync(bytes);
        }

        public void Dispose()
        {
            _isDisposed = true;
            _isAlive = false;

            try
            {
                this.Client?.Dispose();
            }
            catch { }

            Logger.Warn("Connection disposed.", true);
        }
    }
}
