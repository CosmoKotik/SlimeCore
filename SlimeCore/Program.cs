using SlimeCore.Core;
using SlimeCore.Core.Chunks;
using SlimeCore.Network;
using SlimeCore.Tools.Nbt;
using System;

namespace SlimeCore
{
    public class Program
    { 
        public static void Main(string[] args) 
        {
            /*Nbt[] nbt = { NbtBuilder.BuildNbt(NbtType.TAG_STRING, "name", (object)"Bananrama"),
                NbtBuilder.BuildNbt(NbtType.TAG_STRING, "age", (object)"69"),
                NbtBuilder.BuildNbt(NbtType.TAG_STRING, "isFuckable", (object)"yes"),
                NbtBuilder.BuildNbt(NbtType.TAG_STRING, "gay", (object)"MEGA EXTRA GAY")};

            //Nbt[] nbt1 = { NbtBuilder.BuildNbtCompound("SUPER GAY", nbt), NbtBuilder.BuildNbtCompound("MEGA GAY", nbt) };

            Nbt nbtlist = NbtBuilder.BuildNbtCompound("", new Nbt[] { NbtBuilder.BuildNbtList("servers", new Nbt[] { NbtBuilder.BuildNbtCompound("gay", new Nbt[] { NbtBuilder.BuildNbt(NbtType.TAG_STRING, "ip", (object)"10.0.1.3:11000"), NbtBuilder.BuildNbt(NbtType.TAG_STRING, "name", (object)"Minecraft Server") }), NbtBuilder.BuildNbtCompound("gay", new Nbt[] { NbtBuilder.BuildNbt(NbtType.TAG_STRING, "ip", (object)"127.0.0.1:11000"), NbtBuilder.BuildNbt(NbtType.TAG_STRING, "name", (object)"Minecraft Server") }) }) });

            byte[] bytes = nbtlist.GetBytes();

            Console.WriteLine(BitConverter.ToString(bytes).Replace("-", " ") + "   " + bytes.Length);
*/
            //ChunkSection cs = new ChunkSection();
            //cs.Fill(Enums.BlockType.Stone);
            //Console.WriteLine("Received: {0}", BitConverter.ToString(cs.GetBytes()).Replace("-", " "));

            ServerManager sm = new ServerManager("10.0.0.3", 11000);
            sm.Start();

            Console.Read();
        }

        private static byte[] StringToByteArray(string hexc)
        {
            string[] hexValuesSplit = hexc.Split(' ');
            byte[] bytes = new byte[hexValuesSplit.Length];
            int i = 0;
            foreach (string hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}",
                //                    hex, value, stringValue, charValue);
                bytes[i] = (byte)value;
                i++;
            }

            return bytes;
        }
    }
}