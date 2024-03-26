using SlimeCore.Enums;
using SlimeCore.Network;
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

        public Versions ClientVersion { get; set; }

        public bool IsDisposed { get => _isDisposed; }
        private bool _isDisposed = false;

        public bool IsAlive { get => _isAlive; }
        private bool _isAlive = true;

        public ServerManager ServerManager;

        public QueueHandler QueueHandler;

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
                    if (this.Client.Available > 0)
                    {
                        using (var received = this.Client.ReceiveAsync(buffer, SocketFlags.None))
                        {
                            byte[] bytes = new byte[received.Result];
                            Array.Copy(buffer, bytes, received.Result);

                            bm.SetBytes(bytes);

                            int packetSize = bm.GetPacketSize() - 1;
                            int packetID = bm.GetPacketId();

                            byte[] packetBytes = new byte[packetSize];
                            Array.Copy(bm.GetBytes(), packetBytes, packetSize);
                            new PacketByteHandler(ServerManager, this, QueueHandler).HandleBytes(packetBytes);

                            while (bm.GetBytes().Length > packetSize)
                            {
                                bm.RemoveRangeByte(packetSize);

                                packetSize = bm.GetPacketSize() - 1;
                                packetID = bm.GetPacketId();

                                packetBytes = new byte[packetSize];
                                Array.Copy(bm.GetBytes(), packetBytes, packetSize);
                                new PacketByteHandler(ServerManager, this, QueueHandler).HandleBytes(packetBytes);
                            }
                        }

                        await Task.Delay(1);
                    }
                }
            }
        }

        public async void SendAsync(byte[] bytes, bool includeSize = true)
        {
            if (_isDisposed)
            {
                Logger.Warn("Disposed");
                return;
            }

            BufferManager bm = new BufferManager();
            if (includeSize)
                bm.WriteVarInt(bytes.Length);

            bm.InsertBytes(bytes);

            Socket? client;

            lock (this.Client)
                client = this.Client;

            using (client) 
            {
                await client.SendAsync(bytes);
            }
        }
    }
}
