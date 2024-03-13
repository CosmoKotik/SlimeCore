using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Delta.Tools
{
    public static class DLM
    {
        //private static Dictionary<object, LockClass> _locks = new Dictionary<object, LockClass>();
        private static Dictionary<string, int> _lockHashcodes = new Dictionary<string, int>();    //Key = name of object, Value = thread id
        //private static Dictionary<string, LockObject> _lockHashcodes = new Dictionary<string, LockObject>();    //Key = name of object, Value = thread id
        private static object _lock = new object();

        /*public static void TryLock<T>(ref T obj)
        {
            string name = $"{GetCaller()}.{nameof(obj)}";
            //string name = $"{obj.GetType().BaseType}.{nameof(obj)}";

            bool hasObject = false;

            CancellationTokenSource taskCTS = new CancellationTokenSource();

            LockObject lockObject = new LockObject()
            {
                IsLocked = true,
                Locker = new object() { },
                CancellationTokenSource = taskCTS
            };

            lockObject.LockTask = new Task(() => LockTask(lockObject), taskCTS.Token);

            lock (_lock)
            {
                if (!_lockHashcodes.ContainsKey(name))
                    _lockHashcodes.Add(name, lockObject);
                else
                    hasObject = true;
            }

            if (!hasObject)
                lockObject.LockTask.Start();
        }

        private static async void LockTask(LockObject obj)
        {
            lock (obj.Locker)
            {
                while (!obj.CancellationTokenSource.IsCancellationRequested)
                { }
            }
        }

        public static void RemoveLock<T>(ref T obj, int threadId = -1)
        {
            string name = $"{GetCaller()}.{nameof(obj)}";

            lock (_lock)
            {
                if (_lockHashcodes.ContainsKey(name))
                {
                    LockObject locker;
                    _lockHashcodes.TryGetValue(name, out locker);
                    locker.CancellationTokenSource.Cancel();
                    _lockHashcodes.Remove(name);
                }
            }
        }*/

        public static void TryLock<T>(ref T obj, int threadId = -1)
        {
            if (threadId == -1)
                threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"{obj.GetType().BaseType}.{nameof(obj)}");

            //string name = $"{obj.GetType().BaseType}.{nameof(obj)}";
            string name = $"{GetCaller()}.{nameof(obj)}";
            //int hashcode = nameof(obj).GetHashCode() + GetCaller().GetHashCode();
            bool hasObject = false;

            lock (_lock)
            {
                if (!_lockHashcodes.ContainsKey(name))
                    _lockHashcodes.Add(name, threadId);
                else
                    hasObject = true;
            }

            if (!hasObject)
                return;

            while (_lockHashcodes.ContainsKey(name))
                Thread.Sleep(1);
        }

        public static void RemoveLock<T>(ref T obj, int threadId = -1)
        {
            if (threadId == -1)
                threadId = Thread.CurrentThread.ManagedThreadId;

            //string name = $"{obj.GetType().BaseType}.{nameof(obj)}";
            string name = $"{GetCaller()}.{nameof(obj)}";
            //int hashcode = nameof(obj).GetHashCode() + GetCaller().GetHashCode();

            lock (_lock)
            {
                if (_lockHashcodes.ContainsKey(name) &&
                    _lockHashcodes.ContainsValue(threadId))
                        _lockHashcodes.Remove(name, out threadId);
            }
        }

        private static string GetCaller()
        {
            var trace = new StackTrace(2);
            var frame = trace.GetFrame(0);
            var caller = frame.GetMethod();
            var callingClass = caller.DeclaringType.BaseType.Name;
            //var callingClass = caller.DeclaringType.FullName.Split("+")[0];

            return callingClass;
        }

        public static int GetCurrentID<T>()
        { 
            return 0;
        }
    }

    internal class LockClass
    {
        public object Locker { get; set; }
        public bool IsInUse { get; set; }
    }
}
