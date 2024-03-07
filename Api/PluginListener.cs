using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Api
{
    public abstract class PluginListener
    {
        public abstract void OnInit();
        public abstract void OnStop();
        
        public void OnTick() { }
    }
}
