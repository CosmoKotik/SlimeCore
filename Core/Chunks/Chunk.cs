using SlimeCore.Network;
using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks
{
    public class Chunk
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool GroundUpContinuous { get; set; }
        public varint PrimaryBitMask { get; set; }
        public varint DataSize { get; set; }
        public byte[] Data { get; set; }
        public varint NumberOfBlockEntities { get; set; }
        public byte[] BlockEntities { get; set; }


        public void Build(int xPos, int zPos, bool continuous = true, int block_id = 1)
        { 
            this.ChunkX = xPos;
            this.ChunkZ = zPos;
            this.GroundUpContinuous = continuous;

            int mask = 0;
            BufferManager bm = new BufferManager();

            for (int y = 0; y < 8; y++)
            {
                mask |= (1 << y);
                ChunkSection section = ChunkSection.GenerateFill(block_id, false);
                bm.WriteBytes(ChunkSection.GetBytes(section), false);
            }

            if (continuous)
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        bm.WriteByte(127);  //Currently biomes are not supported
                    }
                }

            byte[] data = bm.GetBytes();

            PrimaryBitMask = mask;
            DataSize = data.Length;
            Data = data;

            NumberOfBlockEntities = 0;  //Currently block entities not supported
        }

        public byte[] GetBytes()
        {
            BufferManager bm = new BufferManager();
            
            bm.WriteInt(this.ChunkX);
            bm.WriteInt(this.ChunkZ);
            bm.WriteBool(this.GroundUpContinuous);
            bm.WriteVarInt(this.PrimaryBitMask);
            bm.WriteVarInt(this.DataSize);
            bm.WriteBytes(this.Data, false);
            bm.WriteVarInt(this.NumberOfBlockEntities);

            return bm.GetBytes();
        }
    }
}
