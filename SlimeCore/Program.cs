using SlimeCore.Core;
using SlimeCore.Core.Networking;
using System;

namespace SlimeCore 
{
    public class Program
    {
        public static void Main()
        {
            ServerManager sm = new ServerManager();
            sm.Start();
        }
    }
}