using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Plugin
{
    public class PluginType
    {
        public Assembly Dll { get; set; }
        public List<Type> InvokeTypes { get; set; }
    }
}
