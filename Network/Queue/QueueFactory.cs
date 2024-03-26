using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Queue
{
    public class QueueFactory
    {
        private QueueObject _object;

        public QueueFactory() 
        {
            _object = new QueueObject();
        }
        public QueueFactory SetBytes(byte[] bytes)
        { 
            _object.Bytes = bytes;
            return this;
        }
        public QueueFactory SetClientID(int id)
        { 
            _object.ClientID = id;
            return this;
        }
        public QueueFactory SetQueueID(int id)
        { 
            _object.QueueID = id;
            return this;
        }
        public QueueFactory SetUsedState(bool used)
        { 
            _object.IsUsed = used;
            return this;
        }
        public QueueFactory IsBroadcast(bool includeSelf)
        {
            _object.IsBroadcast = true;
            _object.IncludeSelf = includeSelf;
            return this;
        }

        public QueueObject Build()
        { 
            return _object;
        }
    }
}
