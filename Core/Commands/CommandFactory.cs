using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Commands
{
    public class CommandFactory
    {
        private string _command_name = string.Empty;
        //private string _command = string.Empty;
        private string _success_message = string.Empty;
        private string _failure_message = string.Empty;
        private List<ICommand> _arguments = new List<ICommand>();
        private Delegate _action;
        private List<object> _action_params = new List<object>();

        private Command _command;

        /// <summary>
        /// Please use static method CommandFactory.CreateNew(string command_name) to initiate a new command
        /// </summary>
        /// <param name="command"></param>
        internal CommandFactory(string command)
        {
            this._command_name = command;
        }

        public CommandFactory SetSuccessMessage(string success_message) 
        {
            this._success_message = success_message;
            return this;
        }
        public CommandFactory SetFailureMessage(string failure_message)
        {
            this._failure_message = failure_message;
            return this;
        }
        public CommandFactory AddArgument(ICommand argument)
        {
            this._arguments.Add(argument);
            return this;
        }
        public CommandFactory AddAction(Delegate action)
        {
            _action = action;
            return this;
        }
        public CommandFactory AddActionArgument(object arg)
        {
            _action_params.Add(arg);
            return this;
        }

        public CommandFactory Build()
        {
            Command command = new Command()
            { 
                CommandName = _command_name,
                Success_Message = _success_message,
                Failure_Message = _failure_message,
                Arguments = _arguments.ToArray(),
                Action_Params = _action_params.ToArray(),
                Action = _action
            };

            _command = command;

            return this;
        }

        public Command GetCommand()
        { 
            return _command;
        }

        public Command RegisterCommand()
        {
            Task.Run(() =>
            {
                CommandHandler.AddCommands(_command);
            });

            return _command;
        }

        public static CommandFactory CreateNew(string command_name)
        { 
            return new CommandFactory(command_name);
        }
    }
}
