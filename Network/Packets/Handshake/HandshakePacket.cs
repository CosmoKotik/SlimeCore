using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Handshake
{
    internal class HandshakePacket : IServerboundPacket
    {
        int IPacket.Id { get; set; }
        Version IPacket.Version { get; set; }

        public int ProtocolVersion { get; set; }
        public int Port { get; set; }
        public int NextState { get; set; }
        public string Ip { get; set; }

        public object Read(object[] objs)
        {
            byte[] bytes = objs[0] as byte[];

            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            this.ProtocolVersion = bm.ReadVarInt();
            this.Ip = bm.ReadString();
            this.Port = bm.ReadShort();
            this.NextState = bm.ReadVarInt();

            return this;
        }

        /*public object Read()
        {
            throw new NotImplementedException();
        }*/
    }
}
