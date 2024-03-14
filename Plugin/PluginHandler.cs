using Delta.Tools;
using SlimeApi;
using SlimeApi.Entities;
using SlimeCore.Core;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Plugin
{
    public class PluginHandler
    {
        private ServerManager _serverManager;

        public PluginHandler(ServerManager manager)
        {
            _serverManager = manager;
        }

        public void TickPlugins()
        {
            _serverManager.InvokeAllPluginsMethod(PluginMethods.OnTick);

            PluginObject[] objs = _serverManager.InvokeAllPluginsMethod(PluginMethods.GetPlayers);
            foreach (PluginObject obj in objs) 
            {
                foreach (Player[] players in obj.returnedObjects)
                {
                    foreach (Player player in players)
                    {
                        DLM.TryLock(() => _serverManager.Players);
                        int index = _serverManager.Players.FindIndex(x => x.EntityID == player.EntityID);
                        _serverManager.Players[index] = (SlimeCore.Entities.Player)CastOT.CastToCore(player);
                        DLM.RemoveLock(() => _serverManager.Players);
                    }
                }
            }

            objs = _serverManager.InvokeAllPluginsMethod(PluginMethods.GetEvents);
            foreach (PluginObject obj in objs)
            {
                foreach (PluginEvent[] events in obj.returnedObjects)
                {
                    foreach (PluginEvent pevent in events)
                    {
                        //string[] eventPath = pevent.EventName.Split('.');
                        //DLM.TryLock(ref _serverManager);
                        object e = PluginEventParser.ParseEvent(pevent, new object[] { _serverManager });
                        //DLM.RemoveLock(ref _serverManager);
                    }
                }
            }
        }
    }
}
