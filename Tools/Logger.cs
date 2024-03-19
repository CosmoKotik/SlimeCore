using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Tools
{
    public static class Logger
    {
        public static bool IsDebug { get; set; } = false;

        public static void Log(string msg, bool debug = false)
        {
            if (debug && !IsDebug)
                return;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")} LOG]: {msg}");
        }

        public static void Warn(string msg, bool debug = false)
        {
            if (debug && !IsDebug)
                return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")} WARN]: {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(string msg, bool debug = false)
        {
            if (debug && !IsDebug)
                return;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")} ERROR]: {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
