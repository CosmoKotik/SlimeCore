using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums;
using SlimeCore.Structs;
using System;

namespace SlimeCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Block block = new Block().SetPosition(new Position(43, 18, 21));
            Console.WriteLine(block.GetChunkPosition().ToString());

            ServerManager serverManager = new ServerManager();
            serverManager.Start();

            Console.ReadLine();
        }
    }
}