using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Queue
{
    public class QueuePacket
    {
        //public int PacketID { get; set; }
        public int QueueID { get; set; }
        public int ClientID { get; set; }
        public byte[] Bytes { get; set; }
        public bool IsBroadcast { get; set; }
        public bool IncludeSelf { get; set; }
    }
}
