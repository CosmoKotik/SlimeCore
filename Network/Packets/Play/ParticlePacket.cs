using SlimeCore.Core;
using SlimeCore.Core.Classes;
using SlimeCore.Enums.Clientbound;
using SlimeCore.Network.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Network.Packets.Play
{
    public class ParticlePacket : IClientboundPacket, IPacket
    {
        public int Id { get; set; } = (int)CB_PlayPacketType.PARTICLE;
        public Version Version { get; set; }

        private ClientHandler _handler;

        public ParticlePacket(ClientHandler handler)
        {
            this._handler = handler;
        }

        public object Broadcast(bool includeSelf = false)
        {
            throw new NotImplementedException();
        }
        public object Broadcast(object obj = null, bool includeSelf = false)
        {
            Particle particle = (Particle)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteInt((int)particle.ParticleID);

            bm.WriteBool(particle.IsLongDistance);

            bm.WriteFloat((float)particle.Position.X);
            bm.WriteFloat((float)particle.Position.Y);
            bm.WriteFloat((float)particle.Position.Z);

            bm.WriteFloat((float)particle.Offset.X);
            bm.WriteFloat((float)particle.Offset.Y);
            bm.WriteFloat((float)particle.Offset.Z);

            bm.WriteFloat((float)particle.ParticleData);
            bm.WriteInt(particle.ParticleCount);

            for (int i = 0; i < particle.Data.Count; i++)
                bm.WriteInt(particle.Data[i]);

            _handler.QueueHandler.AddBroadcastPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build(), includeSelf);

            return this;
        }

        public object Write(object obj)
        {
            Particle particle = (Particle)obj;

            BufferManager bm = new BufferManager();
            bm.SetPacketId((byte)Id);

            bm.WriteInt((int)particle.ParticleID);

            bm.WriteBool(particle.IsLongDistance);

            bm.WriteFloat((float)particle.Position.X);
            bm.WriteFloat((float)particle.Position.Y);
            bm.WriteFloat((float)particle.Position.Z);

            bm.WriteFloat((float)particle.Offset.X);
            bm.WriteFloat((float)particle.Offset.Y);
            bm.WriteFloat((float)particle.Offset.Z);

            bm.WriteFloat((float)particle.ParticleData);
            bm.WriteInt(particle.ParticleCount);

            for (int i = 0; i < particle.Data.Count; i++)
                bm.WriteInt(particle.Data[i]);

            _handler.QueueHandler.AddPacket(new QueueFactory().SetBytes(bm.GetBytesWithLength()).Build());

            return this;
        }

        public object Write()
        {
            throw new NotImplementedException();
        }
    }
}
