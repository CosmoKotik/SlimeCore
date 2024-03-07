using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Plugin
{
    public class PluginObject
    {
        public PluginType plugin { get; set; }
        public object[] returnedObjects { get; set; }
    }
}
