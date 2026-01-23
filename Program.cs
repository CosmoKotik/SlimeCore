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
            /*for (int i = 0; i < 4096; i++)
            {
                int x = i & 15;      // i % 16
                int z = (i >> 4) & 15;      // (i / 16) % 16
                int y = i >> 8;             // i / 256

                Console.WriteLine($"x: {x} y: {y} z: {z}");
            }*/

            ServerManager serverManager = new ServerManager();
            serverManager.Start();

            Console.ReadLine();
        }
    }
}