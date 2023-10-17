using SlimeCore.Core;
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
            ServerManager sm = new ServerManager("10.0.0.3", 11000);
            sm.Start();
        }
    }
}