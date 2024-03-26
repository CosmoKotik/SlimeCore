using SlimeCore.Enums;
using SlimeCore.Network;
using SlimeCore.Network.Packets.Handshake;
using SlimeCore.Network.Packets.Status;
using SlimeCore.Network.Queue;
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

        private ClientHandler _clientHandler;
        private QueueHandler _queueHandler;

        public PacketByteHandler(ServerManager serverManager, ClientHandler clientHandler, QueueHandler queueHandler) 
        {
            this._serverManager = serverManager;
            this._configs = serverManager.Config;
            this._clientHandler = clientHandler;
            this._queueHandler = queueHandler;
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

        public bool IsPingPacket(byte[] bytes)
        {
            HandshakePacket packet = (HandshakePacket)new HandshakePacket().Read(new object[] { bytes });

            _clientHandler.ClientVersion = (Versions)packet.ProtocolVersion;

            new Status(_clientHandler).Write();

            if (packet.NextState == 1)
                return true;

            return false;
        }
    }
}
