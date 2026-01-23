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
            ServerManager serverManager = new ServerManager();
            serverManager.Start();

            Console.ReadLine();
        }
    }
}