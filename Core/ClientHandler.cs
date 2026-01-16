using SlimeCore.Core.Classes;
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
        public NetworkListener NetworkListener;

        public MinecraftClient MC_Client;

        public ClientState State { get; set; }

        public ClientHandler(ref ServerManager serverManager, NetworkListener networkListener)
        {
            this.ServerManager = serverManager;
            this.NetworkListener = networkListener;
            this.QueueHandler = new QueueHandler(this);

            this.MC_Client = new MinecraftClient();
            this.MC_Client.ClientHandler = this;


            this.NetworkListener.AddClientHandler(this);
            //this.ServerManager.AddPlayer(this.MC_Client);     //Now in PacketByteHandler in Play state in Client Settings, makes more sense and just better cuz fuck you
            //this.NetworkListener.AddQueueHandler(ref this.QueueHandler);
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
                            HandleDisconnect();
                            this.Dispose();
                        }

                        byte[] bytes = new byte[received.Result];
                        Array.Copy(buffer, bytes, received.Result);
                        //Console.WriteLine("Received==: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);

                        bm.SetBytes(bytes);

                        while (bm.GetBytes().Length > 0)
                        {
                            int packetSize = bm.GetPacketSize();
                            int packetID = bm.GetPacketId();

                            byte[] packetBytes = new byte[packetSize];
                            Array.Copy(bm.GetBytes(), packetBytes, packetSize);
                            new PacketByteHandler(ServerManager, this, QueueHandler, MC_Client).HandleBytes((byte)packetID, packetBytes);

                            bm.RemoveRangeByte(packetSize);
                        }
                    }
                    await Task.Delay(1);
                }

                Console.WriteLine("end");
                this.ServerManager.RemovePlayer(this.MC_Client);
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

            await client.SendAsync(bytes);
        }

        public MinecraftClient[] GetAllPlayers()
        {
            return ServerManager.GetAllPlayers();
        }
        public void HandleDisconnect()
        {
            PlayerListItem playerListItem = new PlayerListItem().SetAction(PlayerListItemAction.REMOVE_PLAYER).SetFromMinecraftClient(this.MC_Client);

            new PlayerListItemPacket(this).Broadcast(playerListItem, false);
            new DestroyEntitiesPacket(this).Broadcast(this.MC_Client, false);
        }

        public void UpdateMinecraftClient(MinecraftClient client)
        { 
            this.MC_Client = client;
        }

        public void Dispose()
        {
            _isDisposed = true;
            _isAlive = false;

            this.NetworkListener.RemoveClientHandler(this);

            try
            {
                this.Client?.Dispose();
                this.QueueHandler?.Dispose();
            }
            catch { }

            Logger.Warn("Connection disposed.", true);

            GC.Collect();
            Logger.Warn("GC called to collect.", true);
        }
    }
}
