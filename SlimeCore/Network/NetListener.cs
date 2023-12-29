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
        private TcpListener _tcpListener;

        private string _ip;
        private int _port;

        public NetListener(ServerManager serverManager)
        { 
            this._serverManager = serverManager;

            this._ip = serverManager.IpAddress;
            this._port = serverManager.Port;

            this._tcpListener = new TcpListener(IPAddress.Parse(this._ip), this._port);
        }

        public void Listen()
        { 
            HandlerAsync().GetAwaiter().GetResult();
        }

        private async Task HandlerAsync()
        {
            Console.WriteLine("Starting a high performace shitty minecraft server!!!!");

            this._tcpListener.Start();

            while (_serverManager.IsStarted)
            { 
                TcpClient client = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                ClientHandler handler = new ClientHandler(client, _serverManager);
                _serverManager.NetClients.Add(handler);
                await Task.Run((Func<Task>)handler.NetworkHandler);
            }
        }
    }
}
