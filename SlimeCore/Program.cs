using SlimeCore.Core;
using System;

namespace SlimeCore
{
    public class Program
    { 
        public static void Main(string[] args) 
        {
            ServerManager sm = new ServerManager("10.0.1.3", 11000);
            sm.Start();
        }
    }
}