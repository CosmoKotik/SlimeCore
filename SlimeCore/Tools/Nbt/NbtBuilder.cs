using SlimeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Tools.Nbt
{
    public static class NbtBuilder
    {
        /// <summary>
        /// Returns an nbt class object
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Nbt BuildNbt(NbtType type)
        {
            return new Nbt()
            {
                NBT_TYPE = type,
                Name = null,
                Payload = null
            };
        }

        /// <summary>
        /// Returns an nbt class object
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Nbt BuildNbt(NbtType type, string name)
        {
            return new Nbt()
            {
                NBT_TYPE = type,
                Name = name,
                Payload = null
            };
        }

        /// <summary>
        /// Returns an nbt class object
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Nbt BuildNbt(NbtType type, string name, object payload) 
        {
            return new Nbt()
            {
                NBT_TYPE = type,
                Name = name,
                Payload = payload
            };
        }

        /// <summary>
        /// Returns a compound nbt
        /// </summary>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Nbt BuildNbtCompound(string name, Nbt[] payload)
        {
            BufferManager bm = new BufferManager();

            Nbt nbt = new Nbt()
            {
                NBT_TYPE = NbtType.TAG_COMPOUND,
                Name = name
            };

            for (int i = 0; i < payload.Length; i++)
                bm.InsertBytes(payload[i].GetBytes());

            bm.InsertBytes(BuildNbt(NbtType.TAG_END).GetBytes());

            nbt.Payload = bm.GetBytes();

            return nbt;
        }

        /// <summary>
        /// Returns a list nbt
        /// </summary>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Nbt BuildNbtList(string name, Nbt[] payload)
        {
            BufferManager bm = new BufferManager();

            Nbt nbt = new Nbt()
            {
                NBT_TYPE = NbtType.TAG_LIST,
                Name = name
            };

            if (payload.Length > 0)
            {
                bm.AddByte((byte)payload[0].NBT_TYPE);
                bm.AddInt(payload.Length, true);
            }

            for (int i = 0; i < payload.Length; i++)
            {
                bm.InsertBytes(payload[i].GetBytes(false, false));
            }

            //bm.InsertBytes(BuildNbt(NbtType.TAG_END).GetBytes());
            bm.InsertBytes(BuildNbt(NbtType.TAG_END).GetBytes());

            nbt.Payload = bm.GetBytes();

            return nbt;
        }

        /*/// <summary>
        /// Returns nbt bytes for a short
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, short payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddShort(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for an int
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, int payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddInt(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for a long
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, long payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddLong(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for a float
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, float payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddFloat(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for a double
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, double payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddDouble(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for a byte array
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, byte[] payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddBytes(payload);

            return bm.GetBytes();
        }

        /// <summary>
        /// Returns nbt bytes for a string
        /// </summary>
        /// <param name="nbt"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] BuildNbt(NbtType nbt, string name, string payload)
        {
            BufferManager bm = new BufferManager();
            bm.AddByte((byte)nbt);

            if (nbt == NbtType.TAG_END)
                return bm.GetBytes();

            bm.AddString(name);
            bm.AddString(payload);

            return bm.GetBytes();
        }*/
    }
}
