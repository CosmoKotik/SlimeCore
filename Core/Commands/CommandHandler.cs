using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Commands
{
    public class CommandHandler
    {
        private static object _commands_lock = new object();
        public static List<ICommand> Commands { get; private set; }

        private ServerManager _serverManager;

        internal const string WRONG_OR_INCOMPLETE_COMMAND_MESSAGE = "Unknown command. Try /help for a list of commands";

        public CommandHandler(ServerManager server_manager, bool initiate_default_commands = true)
        {
            _serverManager = server_manager;

            Commands = new List<ICommand>();

            if (initiate_default_commands)
                InitiateCommands();
        }

        private void InitiateCommands()
        {
            CommandFactory.CreateNew("gamemode")
                .SetFailureMessage("/gamemode <mode> [player]")
                .AddArgument(CommandFactory.CreateNew("survival")
                                           .SetSuccessMessage("Your game mode has been updated to Survival Mode")
                                           .AddAction(_serverManager.ChangePlayerGamemode)
                                           .AddActionArgument(Gamemode.SURVIVAL)
                                           .Build().GetCommand())
                .AddArgument(CommandFactory.CreateNew("creative")
                                           .SetSuccessMessage("Your game mode has been updated to Creative Mode")
                                           .AddAction(_serverManager.ChangePlayerGamemode)
                                           .AddActionArgument(Gamemode.CREATIVE)
                                           .Build().GetCommand())
                .AddArgument(CommandFactory.CreateNew("adventure")
                                           .SetSuccessMessage("Your game mode has been updated to Adventure Mode")
                                           .AddAction(_serverManager.ChangePlayerGamemode)
                                           .AddActionArgument(Gamemode.ADVENTURE)
                                           .Build().GetCommand())
                .AddArgument(CommandFactory.CreateNew("spectator")
                                           .SetSuccessMessage("Your game mode has been updated to Spectator Mode")
                                           .AddAction(_serverManager.ChangePlayerGamemode)
                                           .AddActionArgument(Gamemode.SPECTATOR)
                                           .Build().GetCommand())
                .Build().RegisterCommand();
        }

        internal static void AddCommands(ICommand command) 
        {
            lock (_commands_lock)
                Commands.Add(command);
        }

        public static bool HandleCommand(IEntity caller, string raw_command, out CommandStatus status)
        {
            string[] splitted_command = raw_command.Split(" ");

            status = new CommandStatus()
            {
                Message = WRONG_OR_INCOMPLETE_COMMAND_MESSAGE,
                Status = true
            };

            if (splitted_command.Length < 1)
                return false;

            string command_name = splitted_command[0];
            string[] args = raw_command.Replace($"{command_name} ", "").Split(" ");

            if (!Commands.Any(x => x.CommandName.Equals(command_name)))
                return false;

            Command? command = (Command?)Commands.First(x => x.CommandName.Equals(command_name));

            status = new CommandStatus()
            {
                Command = command,
                Message = command.Failure_Message,
                Status = false
            };

            if (args.Length > 0)
            {
                Command? last_comm_arg = null;
                try
                {
                    for (int i = 0; i < args.Length; i++)
                        last_comm_arg = (Command?)command.Arguments.First(x => x.CommandName.Equals(args[i]));
                }
                catch { }

                if (last_comm_arg == null)
                    return false;

                status = new CommandStatus()
                {
                    Command = last_comm_arg,
                    Message = last_comm_arg.Success_Message,
                    Status = true
                };

                last_comm_arg.ExecuteAction(caller);
                return true;
            }

            if (command == null)
                return false;

            status = new CommandStatus()
            {
                Command = command,
                Message = command.Success_Message,
                Status = true
            };

            command.ExecuteAction(caller);

            return true;
        }

    }
}
