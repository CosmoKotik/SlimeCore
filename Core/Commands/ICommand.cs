using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Commands
{
    public interface ICommand
    {
        public string CommandName { get; set; }
        public ICommand[] Arguments { get; set; }

        public string Success_Message { get; set; }
        public string Failure_Message { get; set; }

        public Delegate Action { get; set; }
        public object[] Action_Params { get; set; }
    }
}
