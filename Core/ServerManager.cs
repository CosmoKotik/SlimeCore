using SlimeCore.Entity;
using SlimeCore.Network;
using SlimeCore.Registry;
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
        public int ViewDistance = 8;
        public int SimulationDistance = 1;

        public bool ReducedDebugInfo = false;
        public bool EnableRespawnScreen = true;
        public bool IsDebug = false;
        public bool IsFlat = false;
        public bool HasDeathLocation = false;
        public int PortalCooldown = 0;

        public int TickPerSecond = 20;
        public float CurrentTPS = 20;

        public bool IsStarted { get; set; }

        public string IpAddress { get; set; }
        public int Port { get; set; }

        public RegistryManager Registry { get; set; }

        public List<ClientHandler> NetClients { get; set; } = new List<ClientHandler>();
        public List<Player> Players { get; set; } = new List<Player>();
        private NetListener _listener;

        public ServerManager(string ip, int port) 
        { 
            this.IpAddress = ip;
            this.Port = port;

            this.Registry = new RegistryManager();
            this.Registry.ParseBlocks("G:\\Dev\\SlimeCore\\Assets\\blocks.json");
            this.Registry.ParseItems("G:\\Dev\\SlimeCore\\Assets\\items.json");

            _listener = new NetListener(this);
        }

        public void Start()
        {
            IsStarted = true;

            new Thread(new ThreadStart(HandleTicksUpdate)).Start();

            _listener.Listen();
        }

        private async void HandleTicksUpdate()
        {
            long lastTickTime = DateTime.UtcNow.Ticks;

            while (IsStarted)
            {
                CurrentTPS = DateTime.UtcNow.Ticks - lastTickTime;
                //Console.WriteLine(CurrentTPS);
                lock (NetClients)
                    NetClients.ForEach(async client => { await client.TickUpdate(); });

                lastTickTime = DateTime.UtcNow.Ticks;
                await Task.Delay(GetWaitTSPTime());
            }
        }

        private int GetWaitTSPTime()
        {
            return 1000 / TickPerSecond;
        }
    }
}
