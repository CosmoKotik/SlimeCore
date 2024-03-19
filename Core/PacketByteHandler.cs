using SlimeCore.Network;
using SlimeCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class PacketByteHandler
    {
        private ServerManager _serverManager;
        private Configs _configs;

        public PacketByteHandler(ServerManager serverManager) 
        {
            this._serverManager = serverManager;
            this._configs = serverManager.Config;
        }

        public void HandleBytes(byte[] bytes)
        {
            if (IsPingPacket(bytes))
            {
                Logger.Log("Received ping packet", true);

                return;
            }

            Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
        }

        public static bool IsPingPacket(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            int protocolVersion = bm.ReadVarInt();
            string ip = bm.ReadString();
            int port = bm.ReadShort();
            int nextState = bm.ReadVarInt();

            if (nextState == 1)
                return true;

            //Console.WriteLine("Received: {0}", BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
            return false;
        }
    }
}
