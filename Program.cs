using SlimeCore.Core;
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