using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Queue
{
    public class QueueFactory
    {
        private QueuePacket _packet = new QueuePacket();

        public QueueFactory SetClientID(int clientID)
        {
            _packet.ClientID = clientID;
            return this;
        }

        public QueueFactory SetBroadcast(bool broadcast, bool includeSelf)
        {
            _packet.IsBroadcast = broadcast;
            _packet.IncludeSelf = includeSelf;
            return this;
        }

        public QueueFactory SetQueueID(int queueID)
        {
            _packet.QueueID = queueID;
            return this;
        }

        public QueueFactory SetBytes(byte[] bytes)
        {
            _packet.Bytes = bytes;
            return this;
        }

        public QueuePacket Build()
        { 
            return _packet;
        }
    }
}
