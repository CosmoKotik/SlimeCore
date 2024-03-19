using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Tools
{
    public class Configs
    {
        public string Motd;
        public string IP;

        public int MaxPlayers;
        public int Port;
        //public int CompressionThreshold;
        public int ConnectionTimeout;
        //public int ReadTimeout;
        //public int GCMemoryActivationThreshold;

        public bool OnlineMode;
        public bool AutoUpdateAPI;
        public bool IsDebug;
        //public bool AllowManualGC;
        //public bool AllowProxy;
        public bool EnforceSecureChat;
    }
}
