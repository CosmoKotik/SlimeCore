using SlimeCore.Core.Entity;
using SlimeCore.Core.Networking;
using SlimeCore.Core.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static SlimeCore.Core.Enums.Difficulty;

namespace SlimeCore.Core
{
    public class ServerManager
    {
        public List<Player> Players = new List<Player>();
        public List<ClientHandler> ClientHandlers = new List<ClientHandler>();

        public IPAddress IP = IPAddress.Parse("10.0.1.3");
        public Int32 Port = 11000;

        public int MaxPlayers = 100;
        public string Motd = "yay!!!";
        public Difficulties Difficutly = Difficulties.easy;

        public ServerManager() { }

        public void Start()
        {
            Listener l = new Listener(this);
            l.Initiate();
        }

        public void AddPlayer(Player player, ClientHandler handler)
        {
            /*for (int i = 0; i < ClientHandlers.Count; i++)
            {
                new PlayerListItem(ClientHandlers[i]) { Action = 0, Latency = 0 }.Write(handler);
                new SpawnPlayer(ClientHandlers[i]).Write();
            }*/
            Players.Add(player);
            ClientHandlers.Add(handler);
            //new PlayerListItem(handler) { Action = 0, Latency = 0 }.Broadcast(true);
           
            for (int i = 0; i < ClientHandlers.Count; i++) 
            {
                if (ClientHandlers[i] != handler)
                {
                    new PlayerListItem(ClientHandlers[i]) { Action = 0, Latency = 0 }.Write(handler);
                    new PlayerListItem(handler) { Action = 0, Latency = 0 }.Write(ClientHandlers[i]);
                    new SpawnPlayer(handler) { Player = ClientHandlers[i].CurrentPlayer }.Write();
                    new SpawnPlayer(ClientHandlers[i]) { Player = player }.Write();
                }
            }
            new PlayerListItem(handler) { Action = 0, Latency = 0 }.Write();
        }

        public void RemovePlayer(Player player, ClientHandler handler)
        {
            new PlayerListItem(handler) { Action = 4, Latency = 0 }.Broadcast(true);
            Players.Remove(player);
            ClientHandlers.Remove(handler);
        }
    }
}
