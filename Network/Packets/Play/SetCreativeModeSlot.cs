using SlimeCore.Enums;
using SlimeCore.Tools.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class SetCreativeModeSlot : IPacket
    {
        public Versions Version { get; set; }
        public int PacketID { get; set; }
        public ClientHandler ClientHandler { get; set; }

        public short SlotIndex { get; set; }
        public bool IsPresent { get; set; }
        public int ItemID { get; set; } = 0;
        public byte ItemCount { get; set; } = 0;
        public Nbt SlotNbt { get; set; }

        public SetCreativeModeSlot(ClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler;
            this.PacketID = PacketHandler.Get(Version, PacketType.SET_CREATIVE_MODE_SLOT);
        }

        public object Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Read(byte[] bytes)
        {
            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            this.SlotIndex = bm.GetShort();
            this.IsPresent = bm.GetBool();

            if (this.IsPresent)
            { 
                this.ItemID = bm.ReadVarInt();
                this.ItemCount = bm.GetByte();
                //this.SlotNbt = NbtBuilder;
            }

            return this;
        }

        public async void Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)PacketID);

            await this.ClientHandler.FlushData(bm.GetBytes());
        }
    }
}
