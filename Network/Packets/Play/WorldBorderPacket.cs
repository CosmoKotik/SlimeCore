using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class WorldBorderPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.WORLD_BORDER;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public WorldBorderPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Write(object obj)
        {   
            Border border = (Border)obj;
            BufferManager bm;

            for (int i = 0; i < border.Actions.Length; i++)
            {
                bm = new BufferManager();
                bm.SetPacketId((byte)Id);

                bm.WriteVarInt(border.Actions[i]);
                
                switch (border.Actions[i]) 
                {
                    case 0:     //Set size
                        bm.WriteDouble(border.Diameter);
                        break;
                    case 2:     //Set center
                        bm.WriteDouble(border.Center.X);
                        bm.WriteDouble(border.Center.Z);
                        break;
                }
                _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());
            }

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}
