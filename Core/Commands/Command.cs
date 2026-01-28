using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Commands
{
    public class Command : ICommand
    {
        public string CommandName { get => _commandName; set => _commandName = value; }
        private string _commandName = string.Empty;
        public ICommand[] Arguments { get => _arguments; set => _arguments = value; }
        private ICommand[] _arguments = new ICommand[0];
        public string Success_Message { get => _success_message; set => _success_message = value; }
        private string _success_message = string.Empty;
        public string Failure_Message { get => _failure_message; set => _failure_message = value; }
        private string _failure_message = string.Empty;
        public Delegate Action { get => _action; set => _action = value; }
        private Delegate _action;

        public object[] Action_Params { get => _action_params; set => _action_params = value; }
        private object[] _action_params = new object[0];

        internal Command ExecuteAction(params object[] args)
        {
            var args_list = args.ToList();
            for (int i = 0; i < _action_params.Length; i++)
                args_list.Add(_action_params[i]);

            args = args_list.ToArray();

            _action.DynamicInvoke(args);

            //Task.Run(() => { _action.Invoke(); });
            return this;
        }
    }
}
