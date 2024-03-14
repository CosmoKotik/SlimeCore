using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets
{
    internal class Handshake : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get => PacketHandler.Get(PacketType.HANDSHAKE); set => throw new NotImplementedException(); }
        public ClientHandler ClientHandler { get; set; }

        public int ProtocolVersion { get; set; }
        public int NextState { get; set; }

        public Handshake(ClientHandler handler)
        {
            this.ClientHandler = handler;
            //PacketID = PacketHandler.Get(Version, PacketType.HANDSHAKE);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public Task Write()
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            ProtocolVersion = bm.ReadVarInt();
            NextState = bm.GetBytes()[bm.GetBytes().Length - 1];

            return this;
        }
    }
}
