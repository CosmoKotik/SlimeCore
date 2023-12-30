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
        Versions Version { get; set; }
        int PacketID { get; set; }
        ClientHandler ClientHandler { get; set; }

        void Write();
        object Read(byte[] bytes);
        void Broadcast(bool includeSelf);
    }
}
