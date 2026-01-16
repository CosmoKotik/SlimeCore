using SlimeCore.Core;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int _poolSize = 1000;
        private ClientHandler _handler;

        private int _poolSizeThreshold = 200;
        private int _burstThreshold = 10;

        private CancellationTokenSource _cancellation;

        public QueueHandler(ClientHandler client) 
        {
            this._handler = client;

            CreatePool(this._poolSize);

            using (_cancellation = new CancellationTokenSource())
            {
                CancellationToken token = _cancellation.Token;
                
                Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        await HandleBytes();

                        /*await Task.Delay(1);*/

                        int queueCount = 0;
                        lock (QueueLockObject)
                            queueCount = this.QueueCount;

                        if (queueCount > _burstThreshold)
                            await this.Delay(1);
                        else
                            await Task.Delay(1);
                    }
                }, token);
            }

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
                {
                    Logger.Warn($"Pool lower than set threshold: {QueuePool.Count}");
                    for (int i = 0; QueuePool.Count < _poolSize; i++)
                        QueuePool.Add(new QueueFactory().SetQueueID(QueuePool.Count - 1).SetUsedState(false).Build());
                }

                //Logger.Warn(QueuePool.Count.ToString(), true);
                //QueuePool.Add(new QueueFactory().SetQueueID(QueuePool.Count - 1).SetUsedState(false).Build());
            }

            if (size < 1)
                return;

            _handler.SendAsync(obj.Bytes);
        }

        public async void AddPacket(QueueObject obj)
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

        public async void AddBroadcastPacket(QueueObject obj, bool includeSelf = false)
        {
            List<QueueHandler> handlers;

            lock (QueueHandlers)
                handlers = QueueHandlers.ToList();

            if (!includeSelf)
                handlers.Remove(this);
            
            foreach (QueueHandler handler in handlers)
                handler.AddPacket(obj);
        }

        public void Dispose()
        {
            lock (QueueHandlers)
                QueueHandlers.Remove(this);
            _cancellation.Cancel();
        }

        private async Task Delay(int microseconds)
        {
            if (microseconds > 1000)
                await Task.Delay(microseconds / 1000);

            long ticks = (long)(microseconds % 1000) * Stopwatch.Frequency / 1_000_000;
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedTicks < ticks) ;
        }
    }
}
