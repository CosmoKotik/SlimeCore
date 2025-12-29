using SlimeCore.Core;
using SlimeCore.Enums;
using SlimeCore.Structs;
using System;

namespace SlimeCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*varint test = int.MaxValue;
            
            Console.WriteLine(test);
            Console.WriteLine(test.Length);*/

            Console.WriteLine(Enum.GetValues(typeof(PacketType)).GetValue(0x00));

            ServerManager serverManager = new ServerManager();
            serverManager.Start();

            Console.ReadLine();
        }
    }
}