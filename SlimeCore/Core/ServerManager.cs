using SlimeCore.Core.Entity;
using SlimeCore.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class ServerManager
    {
        public List<Player> Players = new List<Player>();

        public ServerManager() { }

        public void Start()
        {
            Listener l = new Listener(this);
            l.Initiate();
        }
    }
}
