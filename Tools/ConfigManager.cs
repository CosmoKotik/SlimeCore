using SlimeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tommy;

namespace SlimeCore.Tools
{
    internal static class ConfigManager
    {
        public static Dictionary<string, object> Configs;

        public static Configs Load(ServerManager sm)
        {
            string path = @"config.toml";

            if (!File.Exists(path))
            {
                TomlTable table = new TomlTable()
                {
                    ["Info"] =
                    {
                        ["Version"] = "1.0",
                        ["AutoUpdateAPI"] = sm.AutoUpdateAPI,
                        ["DebugMode"] = sm.IsDebug,
                    },
                    ["Main"] =
                    {
                        ["MaxPlayers"] = sm.MaxPlayers,
                        ["Motd"] = sm.Motd,
                        ["IP"] = sm.IP,
                        ["Port"] = sm.Port,
                        ["OnlineMode"] = sm.OnlineMode,
                    },
                    ["Connection"] =
                    {
                        ["ConnectionTimeout"] = sm.ConnectionTimeout,
                    },
                    /*["GC"] =
                    {
                        ["AllowManualGC"] = pm.AllowManualGC,
                        ["GCMemoryActivationThreshold"] = pm.GCMemoryActivationThreshold,
                    },*/
                    ["Security"] =
                    {
                        ["EnforceSecureChat"] = sm.EnforceSecureChat,
                    }
                };

                using (StreamWriter writer = File.CreateText(path))
                {
                    table.WriteTo(writer);
                    // Remember to flush the data if needed!
                    writer.Flush();
                }

                return null;
            }

            using (StreamReader reader = File.OpenText(path))
            {
                var model = TOML.Parse(reader);

                Configs config = new Configs()
                {
                    MaxPlayers = (int)model["Main"]["MaxPlayers"],
                    ConnectionTimeout = (int)model["Connection"]["ConnectionTimeout"],
                    Port = (int)model["Main"]["Port"],
                    Motd = (string)model["Main"]["Motd"],
                    IP = (string)model["Main"]["IP"],
                    OnlineMode = (bool)model["Main"]["OnlineMode"],
                    AutoUpdateAPI = (bool)model["Info"]["AutoUpdateAPI"],
                    IsDebug = (bool)model["Info"]["DebugMode"],
                    EnforceSecureChat = (bool)model["Security"]["EnforceSecureChat"],
                };

                return config;
            }
        }
    }
}
