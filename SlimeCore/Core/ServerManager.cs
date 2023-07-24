using SlimeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class ServerManager
    {
        public bool IsStarted { get; set; }

        public string IpAddress { get; set; }
        public int Port { get; set; }

        private NetListener _listener;

        public ServerManager(string ip, int port) 
        { 
            this.IpAddress = ip;
            this.Port = port;

            _listener = new NetListener(this);
        }

        public void Start()
        {
            IsStarted = true;

            _listener.Listen();
        }
    }
}
