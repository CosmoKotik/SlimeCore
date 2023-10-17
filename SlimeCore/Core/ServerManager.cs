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
        public int MaxPlayers = 20;
        public int CurrentOnline = 0;
        public int ViewDistance = 2;
        public int SimulationDistance = 1;

        public bool ReducedDebugInfo = false;
        public bool EnableRespawnScreen = true;
        public bool IsDebug = false;
        public bool IsFlat = false;
        public bool HasDeathLocation = false;
        public int PortalCooldown = 0;


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
