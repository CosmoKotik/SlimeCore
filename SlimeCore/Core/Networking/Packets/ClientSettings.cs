using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Networking.Packets
{
    public class ClientSettings
    {
        private int _packetID = 0x04;
        private ClientHandler _handler;

        public ClientSettings(ClientHandler handler)
        { 
            _handler = handler;
        }

        public void Read()
        { 
            
        }
    }
}
