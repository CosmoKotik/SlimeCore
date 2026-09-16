using System;
using SlimeCore.Core;
using SlimeCore.Enums;
using SlimeCore.Enums.Serverbound;
using SlimeCore.Network.Queue;
using SlimeCore.Tools;

namespace SlimeCore.Network.Packets.Play
{
	public class CraftRecipeRequestPacket: IServerboundPacket, IPacket
	{
        public int Id { get; set; } = (int)SB_PlayPacketType.CRAFT_RECIPE_REQUEST;
        public Version Version { get; set; }

        public CraftRecipe Recipe { get; set; }

        private ClientHandler _handler;

        public CraftRecipeRequestPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }

        public object Write(object obj)
        {
            throw new NotImplementedException();
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
            //throw new NotImplementedException();
        }

        public object Read(object[] objs)
        {
            byte[] bytes = objs[0] as byte[];

            BufferManager bm = new BufferManager();
            bm.SetBytes(bytes);

            CraftRecipe recipe = new CraftRecipe()
                .SetWindowID(bm.ReadByte())
                .SetRecipe(bm.ReadVarInt())
                .SetMakeAll(bm.ReadBool());

            this.Recipe = recipe;
                        
            return this;
        }
    }
}

