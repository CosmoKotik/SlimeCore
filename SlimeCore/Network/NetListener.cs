using SlimeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    public class NetListener
    {
        private ServerManager _serverManager;
        //private TcpListener _tcpListener;
        private Socket _socketListener;
        private CancellationTokenSource _listenerCancellationTokenSource = new CancellationTokenSource();

        private bool _isAvailable = true;

        private string _ip;
        private int _port;

        public NetListener(ServerManager serverManager)
        { 
            this._serverManager = serverManager;

            this._ip = serverManager.IpAddress;
            this._port = serverManager.Port;

            //this._tcpListener = new TcpListener(IPAddress.Parse(this._ip), this._port);
        }

        public void Listen()
        { 
            //HandlerAsync().GetAwaiter().GetResult();
            new Thread(() => { HandlerAsync(); }).Start();
        }

        private async void HandlerAsync()
        {
            Console.WriteLine("Starting a high performace shitty minecraft server!!!!");

            using (Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp))
            { 
                this._socketListener = listener;
                _listenerCancellationTokenSource = new CancellationTokenSource();

                try
                {
                    listener.Bind(new IPEndPoint(IPAddress.Parse(this._ip), this._port));
                    listener.Listen(this._port);

                    CancellationToken token = this._listenerCancellationTokenSource.Token;
                    while (_isAvailable)
                    {
                        try
                        {
                            Socket client = await listener.AcceptAsync(token);

                           CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                            await Task.Run(() => 
                            {
                                ClientHandler handler = new ClientHandler(client, _serverManager);

                                handler.NetworkHandler(cancellationTokenSource);
                            }, cancellationTokenSource.Token);

                            /*ClientHandler handler = new ClientHandler(client, _serverManager);
                            _serverManager.NetClients.Add(handler);*/
                            //await Task.Run((Func<Task>)handler.NetworkHandler);
                            //Task.Run(() => handler.NetworkHandler() );
                            //handler.NetworkHandler().GetAwaiter().GetResult();
                            //new Thread(() => { handler.NetworkHandler(); }).Start();
                        }
                        catch { }
                    }
                }
                catch (SocketException e) { Console.WriteLine(e.ToString()); }
                finally
                {
                    listener.Close();
                    listener.Dispose();
                }

                Thread.CurrentThread.Join();
            }

            /*this._tcpListener.Start();

            while (_serverManager.IsStarted)
            { 
                TcpClient client = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                ClientHandler handler = new ClientHandler(client, _serverManager);
                _serverManager.NetClients.Add(handler);
                await Task.Run((Func<Task>)handler.NetworkHandler);
            }*/
        }
    }
}
