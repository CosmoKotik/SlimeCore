using SlimeCore.Core;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    public class NetworkListener
    {
        private CancellationTokenSource _cancellation;
        private Socket _socketListener;

        private ServerManager _serverManager;
        private Configs _config;

        private bool _disposed;

        public List<ClientHandler> ClientHandlers { get; private set; }
        public List<QueueHandler> QueueHandlers { get; private set; }

        public NetworkListener(ServerManager serverManager)
        {
            _serverManager = serverManager;
            _config = _serverManager.Config;

            this.ClientHandlers = new List<ClientHandler>();
            this.QueueHandlers = new List<QueueHandler>();
        }

        public NetworkListener Start()
        {
            Task.Run(() => HandleAsync());
            return this;
        }

        public async Task HandleAsync()
        {
            Logger.Log("Starting a high performance server.");

            using (_cancellation = new CancellationTokenSource())
            {
                CancellationToken token = _cancellation.Token;

                using (Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    _socketListener = listener;

                    try
                    {
                        listener.Bind(new IPEndPoint(IPAddress.Parse(_config.IP), _config.Port));
                        listener.Listen(_config.Port);

                        while (!_disposed)
                        {
                            Socket client = await listener.AcceptAsync(token);

                            Task.Run(async () => { await new ClientHandler(ref _serverManager, this).HandleClient(client); });

                        }
                    }
                    catch (Exception ex) { Logger.Error(ex.Message); }
                    finally
                    {
                        listener.Close();
                        listener.Dispose();
                        Logger.Warn("Listener disposed");
                        _disposed = true;
                    }
                }
            }

            Console.WriteLine("asdasd");
        }

        public void Stop()
        {
            _cancellation.Cancel();
            _disposed = true;
        }

        protected internal NetworkListener AddClientHandler(ClientHandler handler)
        {
            ClientHandlers.Add(handler);
            return this;
        }
        protected internal NetworkListener RemoveClientHandler(ClientHandler handler)
        {
            ClientHandlers.Remove(handler);
            return this;
        }
        protected internal NetworkListener AddQueueHandler(ref QueueHandler handler)
        {
            QueueHandlers.Add(handler);
            return this;
        }
        protected internal List<QueueHandler> GetAllQueueHandlers() => QueueHandlers;
    }
}
