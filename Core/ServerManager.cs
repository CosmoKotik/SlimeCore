﻿using SlimeCore.Network;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class ServerManager
    {
        public string IP { get => _ip; }
        private string _ip = "127.0.0.1";

        public int Port { get => _port; }
        private int _port = 11000;

        public int MaxPlayers { get => _maxPlayers; }
        private int _maxPlayers = 20;

        public int CurrentOnline { get => _currentOnline; }
        private int _currentOnline = 0;

        public int ViewDistance { get => _viewDistance; }
        private int _viewDistance = 8;

        public int SimulationDistance { get => _simulationDistance; }
        private int _simulationDistance = 1;
        
        public int ConnectionTimeout { get => _connectionTimeout; }
        private int _connectionTimeout = 30000;

        public bool AutoUpdateAPI { get => _autoUpdateAPI; }
        private bool _autoUpdateAPI = true;

        public bool ReducedDebugInfo { get => _reducedDebugInfo; }
        private bool _reducedDebugInfo = false;

        public bool IsDebug { get => _isDebug; }
        private bool _isDebug = false;

        public bool OnlineMode { get => _onlineMode; }
        private bool _onlineMode = false;
        
        public bool EnforceSecureChat { get => _enforceSecureChat; }
        private bool _enforceSecureChat = false;

        public string PluginsPath { get => _pluginsPath; }
        private string _pluginsPath = @"plugins";

        public string Motd { get => _motd; }
        private string _motd = @"A slimecore server";

        public int TickPerSecond = 20;
        public float CurrentTPS = 20;
        public string FaviconBase64 { get { return _faviconBase64; } }
        private string _faviconBase64 = "data:image/png;base64,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";

        public Configs Config;

        public NetworkListener NetworkListener;

        public void Start()
        {
            Logger.Log("Loading config...");
            try
            {
                Configs conf = Config = ConfigManager.Load(this);
                if (conf != null)
                {
                    _maxPlayers = conf.MaxPlayers;
                    _connectionTimeout = conf.ConnectionTimeout;
                    _motd = conf.Motd;
                    _port = conf.Port;
                    _ip = conf.IP;
                    _onlineMode = conf.OnlineMode;
                    _autoUpdateAPI = conf.AutoUpdateAPI;
                    _enforceSecureChat = conf.EnforceSecureChat;
                    _isDebug = conf.IsDebug;

                    Logger.Log("Config loaded successfully.");
                }
                else
                    Logger.Log("Config generated successfully.");
            }
            catch (Exception e) { Logger.Error($"Failed to load config: {e}"); return; }

            if (IsDebug)
            {
                Logger.IsDebug = _isDebug;
                Logger.Warn("Debug mode is on", true);
            }

            if (this.AutoUpdateAPI)
            {
                //Check for update code
                Logger.Log("Checking for updates...");
                Logger.Log("No updates");   //temporary lol
            }

            this.NetworkListener = new NetworkListener(this).Start();
        }
    }
}
