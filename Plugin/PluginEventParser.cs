using Delta.Core;
using Delta.Tools;
using SlimeApi;
using SlimeApi.Entities;
using SlimeCore.Core;
using SlimeCore.Core.Metadata;
using SlimeCore.Network;
using SlimeCore.Network.Packets.Play;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Plugin
{
    public class PluginEventParser
    {
        public static object ParseEvent(PluginEvent pevent, object[] args = null)
        {
            string[] eventPath = pevent.EventName.Split('.');

            ServerManager manager = args[0] as ServerManager;
            Guid playerUUID;
            Position playerPosition;
            Entity entity;
            NPC npc;
            SlimeCore.Entities.Player player;

            DLM.TryLock(ref manager.NetClients);
            List<ClientHandler> netclients = new List<ClientHandler>(manager.NetClients);
            DLM.RemoveLock(ref manager.NetClients);

            switch (eventPath[0])
            {
                case "player":

                    playerUUID = Guid.Parse(eventPath[1]);
                    playerPosition = (Position)ParseEvent(new PluginEvent()
                    {
                        EventName = eventPath[2],
                        EventObject = pevent.EventObject
                    });

                    player = manager.Players.Find(x => x.UUID == playerUUID);

                    /*manager.NetClients.ForEach(x =>
                    {
                        new UpdateEntityPosition(x).Write(player);
                    });*/
                    player.CurrentPosition = (SlimeCore.Entities.Position)CastOT.CastToCore(playerPosition);
                    Logger.Warn(playerPosition.PositionX.ToString());

                    break;
                case "entity":
                    switch (eventPath[1])
                    {
                        case "spawn":
                            entity = pevent.EventObject as SlimeApi.Entities.Entity;

                            manager.Entities.Add((Entities.Entity)CastOT.CastToCore(entity));

                            //entity.CustomName = "minecraft:cobblestone";
                            //entity.BlockDisplay = 9;

                            //mdata.AddMetadata("CustomName", Core.Metadata.MetadataType.OptChat, Core.Metadata.MetadataValue.CustomName);
                            //mdata.AddMetadata("IsCustomNameVisible", Core.Metadata.MetadataType.Boolean, Core.Metadata.MetadataValue.IsCustomNameVisible);


                            /*Core.Metadata.Metadata mdata = new Core.Metadata.Metadata();
                            mdata.AddMetadata("BlockDisplay", Core.Metadata.MetadataType.BlockID, Core.Metadata.MetadataValue.BlockDisplay, 9);*/
                            
                            
                            /*mdata.AddMetadata("Width", Core.Metadata.MetadataType.Float, Core.Metadata.MetadataValue.Width);
                            mdata.AddMetadata("Height", Core.Metadata.MetadataType.Float, Core.Metadata.MetadataValue.Height);*/

                            netclients.ForEach(x =>
                            {
                                new SpawnEntity(x).Write((Entities.Entity)CastOT.CastToCore(entity));
                                new SetEntityMetadata(x).Write((Entities.Entity)CastOT.CastToCore(entity), (Core.Metadata.Metadata)CastOT.CastToCore(entity.Metadata));
                            });

                            manager.InvokeAllPluginsMethod(PluginMethods.AddEntity, new object[] { entity });
                            break;
                        case "destroy":
                            Entities.Entity coreEntity = manager.Entities.Find(x => x.EntityID.Equals((int)pevent.EventObject));

                            manager.InvokeAllPluginsMethod(PluginMethods.RemoveEntity, new object[] { coreEntity });

                            netclients.ForEach(x =>
                            {
                                Logger.Error("Remove entity");
                                new RemoveEntities(x).Write(coreEntity);
                            });

                            manager.Entities.Remove(coreEntity);
                            break;
                        case "setposition":
                            entity = pevent.EventObject as SlimeApi.Entities.Entity;

                            netclients.ForEach(x =>
                            {
                                new TeleportEntity(x).Write((Entities.Entity)CastOT.CastToCore(entity));
                            });

                            manager.Entities.Find(x => x.EntityID.Equals(entity.EntityID)).CurrentPosition = (Entities.Position)CastOT.CastToCore(entity.CurrentPosition);
                            break;
                    }
                    break;
                case "npc":
                    switch (eventPath[1])
                    {
                        case "create":
                            npc = pevent.EventObject as SlimeApi.Entities.NPC;
                            npc.isNpc = true;
                            player = (SlimeCore.Entities.Player)CastOT.CastToCore(npc.BuildPlayer());

                            //manager.Players.Add(player);
                            manager.Npcs.Add((Entities.NPC)CastOT.CastToCore(npc));

                            manager.InvokeAllPluginsMethod(PluginMethods.AddNpc, new object[] { npc });

                            //DLM.TryLock(ref manager.NetClients);
                            netclients.ForEach(x =>
                            {
                                new PlayerInfoUpdate(x).AddPlayer(player).Write();
                                new SpawnPlayer(x).Write(player);
                            });
                            //DLM.RemoveLock(ref manager.NetClients);
                            break;
                        case "remove":
                            npc = pevent.EventObject as SlimeApi.Entities.NPC;
                            npc.isNpc = true;
                            player = (SlimeCore.Entities.Player)CastOT.CastToCore(npc.BuildPlayer());

                            //manager.Players.Add(player);
                            manager.Npcs.Remove((Entities.NPC)CastOT.CastToCore(npc));

                            manager.InvokeAllPluginsMethod(PluginMethods.RemoveNpc, new object[] { npc });

                            //DLM.TryLock(ref manager.NetClients);
                            netclients.ForEach(x =>
                            {
                                new PlayerInfoUpdate(x).AddPlayer(player).Write();
                                new SpawnPlayer(x).Write(player);
                            });
                            //DLM.RemoveLock(ref manager.NetClients);
                            break;
                        case "setpos":
                            npc = pevent.EventObject as SlimeApi.Entities.NPC;
                            player = (SlimeCore.Entities.Player)CastOT.CastToCore(npc.BuildPlayer());

                            netclients.ForEach(x =>
                            {
                                new TeleportEntity(x).Write(player);
                            });

                            manager.Npcs.Find(x => x.EntityID.Equals(npc.EntityID)).CurrentPosition = (Entities.Position)CastOT.CastToCore(npc.CurrentPosition);
                            break;
                    }

                    break;
                case "position":
                    Position pos = (Position)pevent.EventObject;
                    return pos;
            }

            return null;
        }
    }
}
