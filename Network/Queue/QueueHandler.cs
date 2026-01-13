using SlimeCore.Core;
using SlimeCore.Tools;
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

        private int _poolSizeThreshold = 30;

        public QueueHandler(ClientHandler client) 
        {
            this._handler = client;

            CreatePool(this._poolSize);

            Task.Run(async () => 
            {
                while (true)
                {
                    await HandleBytes();

                    await Task.Delay(1);
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

            int index = 0;
            QueueObject obj = new QueueObject();

            lock (QueueLockObject)
            {
                if (QueueCount < 1)
                    return;

                size = QueueCount;
                QueueCount--;

                index = QueuePool.First(x => x.IsUsed).QueueID;
                obj = QueuePool.First(x => x.QueueID.Equals(index));
                QueuePool.RemoveAll(x => x.QueueID.Equals(index));

                if (QueuePool.Count <= _poolSizeThreshold)
                    for (int i = 0; QueuePool.Count < _poolSize; i++)
                        QueuePool.Add(new QueueFactory().SetQueueID(QueuePool.Count - 1).SetUsedState(false).Build());

                //Logger.Warn(QueuePool.Count.ToString(), true);
                //QueuePool.Add(new QueueFactory().SetQueueID(QueuePool.Count - 1).SetUsedState(false).Build());
            }

            if (size < 1)
                return;

            _handler.SendAsync(obj.Bytes);
        }

        public void AddPacket(QueueObject obj)
        {
            lock (QueueLockObject)
            {
                QueueCount++;
                /*QueueObject? bufObj = QueuePool.First(x => !x.IsUsed);
                if (bufObj == null) return;*/

                int index = QueuePool.First(x => !x.IsUsed).QueueID;

                obj.QueueID = index;
                obj.IsUsed = true;

                QueuePool.First(x => x.QueueID.Equals(index)).CopyFrom(obj);
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
