using SlimeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums
{
    public interface IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        Task Write();
        object Read(byte[] bytes);
        object Broadcast(bool includeSelf);
    }
}
