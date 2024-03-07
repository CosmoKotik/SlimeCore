using Delta.Core;
using SlimeApi;
using SlimeCore.Entity;
using SlimeCore.Network;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Plugin;
using SlimeCore.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class ServerManager
    {
        public int MaxPlayers { get => _maxPlayers; }
        private int _maxPlayers = 20;
        
        public int CurrentOnline { get => _currentOnline; }
        private int _currentOnline = 0;
        
        public int ViewDistance { get => _viewDistance; }
        private int _viewDistance = 8;
        
        public int SimulationDistance { get => _simulationDistance; }
        private int _simulationDistance = 1;

        public bool ReducedDebugInfo { get => _reducedDebugInfo; }
        private bool _reducedDebugInfo = false;
        
        public bool EnableRespawnScreen { get => _enableRespawnScreen; }
        private bool _enableRespawnScreen = true;
        
        public bool IsDebug { get => _isDebug; }
        private bool _isDebug = false;
        
        public bool IsFlat { get => _isFlat; }
        private bool _isFlat = false;
        
        public bool HasDeathLocation { get => _hasDeathLocation; }
        private bool _hasDeathLocation = false;
        
        public int PortalCooldown { get => _portalCooldown; }
        private int _portalCooldown = 0;

        public string PluginsPath { get => _pluginsPath; }
        private string _pluginsPath = @"plugins";

        public int TickPerSecond = 20;
        public float CurrentTPS = 20;

        //public List<Assembly> Plugins { get => _plugins; }
        //private List<Assembly> _plugins = new List<Assembly>();

        public List<PluginType> Plugins { get => _plugins; }
        private List<PluginType> _plugins = new List<PluginType>();

        public bool IsStarted { get; set; }

        public string IpAddress { get; set; }
        public int Port { get; set; }

        public RegistryManager Registry { get; set; }
        public List<Block> BlockPlaced { get; set; }

        public List<ClientHandler> NetClients { get; set; } = new List<ClientHandler>();
        public List<Player> Players { get; set; } = new List<Player>();
        private NetListener _listener;

        public ServerManager(string ip, int port) 
        { 
            this.IpAddress = ip;
            this.Port = port;

            if (!Directory.Exists(_pluginsPath))
            {
                Logger.Warn("Missing plugins folder...");
                Logger.Warn("Creating plugin folder.");
                Directory.CreateDirectory(_pluginsPath);
            }

            foreach (string filepath in Directory.GetFileSystemEntries(_pluginsPath))
            {
                string fullpath = Path.GetFullPath(filepath);
                var dll = Assembly.LoadFrom(fullpath);

                InvokePluginMethod(AddPlugin(dll), PluginMethods.OnInit);
                
                //PluginObject objs = InvokePluginMethod(AddPlugin(dll), PluginMethods.AddPlayer);

                /*for (int i = 0; i < objs.returnedObjects.Length; i++)
                {
                    Console.WriteLine(objs.returnedObjects[i].ToString());
                }*/
            }

            this.BlockPlaced = new List<Block>();

            this.Registry = new RegistryManager(this);
            //this.Registry.ParseBlocks("G:\\Dev\\SlimeCore\\Assets\\blocks.json");
            //this.Registry.ParseItems("G:\\Dev\\SlimeCore\\Assets\\items.json");

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

                PluginObject[] objs = InvokeAllPluginsMethod(PluginMethods.GetPlayers);

                for (int i = 0; i < objs.Length; i++)
                {
                    for (int x = 0; x < objs[i].returnedObjects.Length; x++)
                    {
                        SlimeApi.Entity.Player[] players = (SlimeApi.Entity.Player[])objs[i].returnedObjects[x];
                        foreach (SlimeApi.Entity.Player player in players)
                            Console.WriteLine(player.Username);
                    }
                }

                lastTickTime = DateTime.UtcNow.Ticks;
                await Task.Delay(GetWaitTSPTime());
            }
        }

        private int GetWaitTSPTime()
        {
            return 1000 / TickPerSecond;
        }

        public PluginType AddPlugin(Assembly dll)
        {
            List<Type> invokedTypes = new List<Type>();
            foreach (var type in dll.GetExportedTypes())
                if (!type.Name.Equals(typeof(PluginListener).Name) && type.BaseType.Name.Equals(typeof(PluginListener).Name))
                    invokedTypes.Add(type);

            PluginType plugin = new PluginType()
            {
                Dll = dll,
                InvokeTypes = invokedTypes
            };
            _plugins.Add(plugin);
            return plugin;
        }

        public PluginObject InvokePluginMethod(PluginType plugin, PluginMethods method, object[] args = null)
        {
            List<object> objs = new List<object>();
            plugin.InvokeTypes.ForEach(t => 
            {
                var instance = Activator.CreateInstance(t);
                objs.Add(t.InvokeMember(Enum.GetName(typeof(PluginMethods), method), BindingFlags.InvokeMethod, null, instance, args));
                /*switch (method)
                {
                    case PluginMethods.OnInit:
                        t.InvokeMember("OnInit", BindingFlags.InvokeMethod, null, instance, null);
                        break;
                    case PluginMethods.OnStop:
                        t.InvokeMember("OnStop", BindingFlags.InvokeMethod, null, instance, null);
                        break;
                    case PluginMethods.OnTick:
                        t.InvokeMember(Enum.GetName(typeof(PluginMethods), method), BindingFlags.InvokeMethod, null, instance, null);
                        break;
                }*/
            });

            return new PluginObject()
            {
                returnedObjects = objs.ToArray(),
                plugin = plugin
            };
        }

        public PluginObject[] InvokeAllPluginsMethod(PluginMethods method, object[] args = null)
        {
            List<PluginObject> objs = new List<PluginObject>();
            Plugins.ForEach(t =>
            {
                objs.Add(InvokePluginMethod(t, method, args));
            });
            return objs.ToArray();
        }
    }
}
