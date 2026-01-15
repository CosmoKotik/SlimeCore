using SlimeCore.Core;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class PlayerPositionAndLookPacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.PLAYER_POSITION_AND_LOOK;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public PlayerPositionAndLookPacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Write(object obj)
        {
            throw new NotImplementedException();
        }

        public object Write()
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteDouble(0);      //X
            bm.WriteDouble(0);      //Y
            bm.WriteDouble(0);      //Z
            bm.WriteFloat(0);       //Yaw
            bm.WriteFloat(0);       //Pitch
            bm.WriteByte(0x02);     //Flags
            bm.WriteVarInt(0);      //Teleport ID

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write(Position pos)
        {
            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteDouble(pos.X);      //X
            bm.WriteDouble(pos.Y);      //Y
            bm.WriteDouble(pos.Z);      //Z
            bm.WriteFloat(0);       //Yaw
            bm.WriteFloat(0);       //Pitch
            bm.WriteByte(0x08);     //Flags
            bm.WriteVarInt(0);      //Teleport ID

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        object IClientboundPacket.Broadcast(bool includeSelf)
        {
            throw new NotImplementedException();
        }

        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}