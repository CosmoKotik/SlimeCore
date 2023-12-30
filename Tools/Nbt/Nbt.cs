using SlimeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeCore.Tools.Nbt
{
    public class Nbt
    {
        public NbtType NBT_TYPE { get; set; }
        public string Name { get; set; }
        public object Payload { get; set; }

        public byte[] PrefixBytes { get; set; } = null;

        public byte[] GetBytes(bool includeName = true, bool includeType = true)
        {
            BufferManager bm = new BufferManager();

            if (includeType)
                bm.AddByte((byte)this.NBT_TYPE);

            if (this.NBT_TYPE == NbtType.TAG_END)
                return bm.GetBytes();

            if (includeName && this.Name != null)
                bm.AddString(this.Name, true);

            if (PrefixBytes != null)
                bm.InsertBytes(PrefixBytes);

            if (this.Payload == null)
                return bm.GetBytes();

            switch (this.NBT_TYPE)
            {
                case NbtType.TAG_BYTE:
                    bm.AddByte((byte)this.Payload);
                    break;
                case NbtType.TAG_SHORT:
                    bm.AddShort((short)this.Payload);
                    break;
                case NbtType.TAG_INT:
                    bm.AddInt((int)this.Payload);
                    break;
                case NbtType.TAG_LONG:
                    bm.AddLong((long)this.Payload);
                    break;
                case NbtType.TAG_FLOAT:
                    bm.AddFloat((float)this.Payload);
                    break;
                case NbtType.TAG_DOUBLE:
                    bm.AddDouble((double)this.Payload);
                    break;
                case NbtType.TAG_BYTE_ARRAY:
                    bm.AddBytes((byte[])this.Payload);
                    break;
                case NbtType.TAG_STRING:
                    bm.AddString((string)this.Payload, true);
                    break;
                case NbtType.TAG_LIST:
                    bm.InsertBytes((byte[])this.Payload);
                    break;
                case NbtType.TAG_COMPOUND:
                    bm.InsertBytes((byte[])this.Payload);
                    break;
            }

            return bm.GetBytes();
        }
    }
}
