using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Queue
{
    public class QueueObject
    {
        public int QueueID { get; set; }
        public int ClientID { get; set; }
        public bool IsBroadcast { get; set; }
        public bool IncludeSelf { get; set; }
        public bool IsUsed { get; set; } = true;
        public byte[] Bytes { get; set; }
    }
}
