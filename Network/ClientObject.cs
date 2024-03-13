using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network
{
    public class ClientObject
    {
        public int ClientID { get; set; }
        public ClientHandler Handler { get; set; }
    }
}
