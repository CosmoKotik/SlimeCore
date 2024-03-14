using Delta.Core;
using Delta.Tools;
using SlimeCore.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Queue
{
    public delegate int QueueHandlerCallback();
    public class QueueHandler
    {
        private static List<QueuePacket>? _packets;
        private static object _packetsLock = new object();

        private ServerManager _serverManager;

        private bool _disposed;

        public QueueHandler(ServerManager manager) 
        {
            _packets = new List<QueuePacket>();
            _serverManager = manager;

            Task.Run(HandlePackets);
            //new Thread(new ThreadStart(HandlePackets)).Start();
        }

        public async void HandlePackets()
        {
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            while (!_disposed)
            {
                /*if (sw.Elapsed.Ticks % 10000 == 0)
                    HandlePacketBytes();*/
                await HandlePacketBytes();
                await Task.Delay(1);
                //Thread.Sleep(1);
            }
            sw.Stop();
        }

        private async Task HandlePacketBytes()
        {
            QueuePacket packet;
            DLM.TryLock(() => _packets);
            if (_packets.Count < 1)
            {
                DLM.RemoveLock(() => _packets);
                return;
            }

            //Logger.Error(_packets.Count.ToString());
            packet = _packets[0];
            //lock(_packetsLock)
            _packets.RemoveAt(0);
            //_packets.Clear();
            DLM.RemoveLock(() => _packets);

            if (packet == null)
                return;

            if (packet.IsBroadcast)
            {
                foreach (var item in _serverManager.GetAllClients())
                {
                    if (item.ClientID.Equals(packet.ClientID) && !packet.IncludeSelf)
                        continue;
                    await item.FlushData(packet.Bytes);
                }

                return;
            }


            ClientHandler? handler = _serverManager.GetClient(packet.ClientID);
            if (handler == null)
                return;

            DLM.TryLock(() => handler.IsDisposed);
            if (handler.IsDisposed)
            {
                DLM.TryLock(() => _packets);
                Logger.Error($"Deleted disposed data: {_packets.FindAll(x => x.ClientID.Equals(handler.ClientID)).Count}");
                _packets.RemoveAll(x => x.ClientID.Equals(handler.ClientID));
                DLM.RemoveLock(() => _packets);
                DLM.RemoveLock(() => handler.IsDisposed);

                return;
            }
            DLM.RemoveLock(() => handler.IsDisposed);

            //await Task.Run(() => { handler.FlushData(packet.Bytes).GetAwaiter(); });
            await handler.FlushData(packet.Bytes);

            /*foreach (var packet in packets)
            {
                if (packet.IsBroadcast)
                {
                    foreach (var item in _serverManager.GetAllClients())
                    {
                        if (item.ClientID.Equals(packet.ClientID) && !packet.IncludeSelf)
                            continue;
                        await item.FlushData(packet.Bytes);
                    }

                    return;
                }


                ClientHandler? handler = _serverManager.GetClient(packet.ClientID);
                if (handler == null)
                    return;

                DLM.TryLock(() => handler.IsDisposed);
                if (handler.IsDisposed)
                {
                    DLM.TryLock(() => _packets);
                    Logger.Error($"Deleted disposed data: {_packets.FindAll(x => x.ClientID.Equals(handler.ClientID)).Count}");
                    _packets.RemoveAll(x => x.ClientID.Equals(handler.ClientID));
                    DLM.RemoveLock(() => _packets);
                    DLM.RemoveLock(() => handler.IsDisposed);

                    return;
                }
                DLM.RemoveLock(() => handler.IsDisposed);

                //await Task.Run(() => { handler.FlushData(packet.Bytes).GetAwaiter(); });
                await handler.FlushData(packet.Bytes);
            }*/
        }

        public static async Task AddPacket(QueuePacket packet)
        {
            DLM.TryLock(() => _packets);
            
            _packets.Add(packet);

            DLM.RemoveLock(() => _packets);
        }

        public static int GetLength()
        {
            DLM.TryLock(() => _packets);
            int length = 0;
            if (_packets == null)
                return 0;

            length = _packets.Count;
            DLM.RemoveLock(() => _packets);
            return length;
        }

        public void Dispose()
        { 
            _disposed = true;
        }
    }
}
