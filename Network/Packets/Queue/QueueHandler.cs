using SlimeCore.Core;
using System;
using System.Collections.Generic;
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
        }

        public async void HandlePackets()
        {
            while (!_disposed)
            {
                HandlePacketBytes();
                //await Task.Delay(1);
            }
        }

        private async void HandlePacketBytes()
        {
            QueuePacket packet;
            lock (_packetsLock) 
            {
                if (_packets == null)
                    return;

                if (_packets.Count < 1)
                    return;

                packet = _packets[0];
                _packets.RemoveAt(0);
            }

            if (packet.IsBroadcast)
            {
                foreach (var item in _serverManager.GetAllClients())
                {
                    if (!packet.IncludeSelf && item.ClientID.Equals(packet.ClientID))
                        continue;
                    await item.FlushData(packet.Bytes);
                }

                return;
            }

            ClientHandler? handler = _serverManager.GetClient(packet.ClientID);
            if (handler == null)
                return;

            //await Task.Run(() => { handler.FlushData(packet.Bytes).GetAwaiter(); });
            await handler.FlushData(packet.Bytes);
        }

        public static void AddPacket(QueuePacket packet)
        {
            lock (_packetsLock)
            {
                if (_packets == null)
                    return;

                _packets.Add(packet);
            }
        }

        public static int GetLength()
        {
            int length = 0;
            lock (_packetsLock)
            {
                if (_packets == null)
                    return 0;

                length = _packets.Count;
            }

            return length;
        }

        public void Dispose()
        { 
            _disposed = true;
        }
    }
}
