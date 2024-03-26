using SlimeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Queue
{
    public class QueueHandler
    {
        public static List<QueueHandler> QueueHandlers = new List<QueueHandler>();

        public List<QueueObject> QueuePool { get; set; } = new List<QueueObject>();
        public int QueueCount { get; set; }
        public object QueueLockObject = new object();

        private int _poolSize = 50;
        private ClientHandler _handler;

        public QueueHandler(ClientHandler client) 
        {
            this._handler = client;

            CreatePool(this._poolSize);

            Task.Run(async () => 
            {
                while (true)
                {
                    await HandleBytes();
                }
            });

            QueueHandlers.Add(this);
        }

        private void CreatePool(int poolSize)
        { 
            for (int i = 0; i < poolSize; i++)
                QueuePool.Add(new QueueFactory().SetQueueID(i).SetUsedState(false).Build());
        }

        private async Task HandleBytes()
        {
            int size = 0;

            lock (QueueLockObject)
            {
                size = QueueCount;
                QueueCount--;
            }

            if (size < 1)
                return;

            QueueObject obj = new QueueObject();

            lock (QueueLockObject)
            {
                obj = QueuePool.First(x => x.IsUsed);
                _handler.SendAsync(obj.Bytes);
                QueuePool.First(x => !x.IsUsed).IsUsed = false;
            }
        }

        public void AddPacket(QueueObject obj)
        {
            lock (QueueLockObject)
            {
                QueueCount++;
                int index = QueuePool.First(x => !x.IsUsed).QueueID;
                QueuePool[index] = obj;
                QueuePool[index].QueueID = index;
            }
        }

        public static Task AddBroadcastPacket(QueueObject obj)
        {
            return Task.Run(() => 
            {
                List<QueueHandler> handlers;

                lock (QueueHandlers)
                    handlers = QueueHandlers;

                foreach (QueueHandler handler in handlers)
                    handler.AddPacket(obj);
            });
        }
    }
}
