using SlimeCore.Core.Networking;
using SlimeCore.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.World
{
    internal class Chunk
    {
        public byte[] GetWorldBytes()
        {
            List<byte> buffer = new List<byte>();
            BufferManager bufferManager = new BufferManager();
            bufferManager.WriteInt(0, ref buffer);
            bufferManager.WriteInt(0, ref buffer);
            bufferManager.WriteBool(true, ref buffer);
            bufferManager.WriteUShort(0, ref buffer);
            bufferManager.WriteVarInt(0, ref buffer);

            return buffer.ToArray();
        }
    }
}
