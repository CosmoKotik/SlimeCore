using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Chunks.Loader
{
    public class RegionFile
    {
        private readonly string _path = string.Empty;

        public RegionFile(string path)
        { 
            //_stream = File.OpenRead(path);
            _path = path;
        }

        public byte[] ReadChunk(int x, int z)
        {
            using (var stream = File.OpenRead(_path))
            {
                int index = x + z * 32;
                stream.Seek(index * 4, SeekOrigin.Begin);

                byte[] loc = new byte[4];
                stream.Read(loc, 0, 4);

                int offset = (loc[0] << 16 | loc[1] << 8 | loc[2]) * 4096;
                int size = loc[3] * 4096;

                if (offset == 0 || size == 0)
                    return null;

                stream.Seek(offset, SeekOrigin.Begin);

                byte[] lenBytes = new byte[4];
                stream.Read(lenBytes, 0, 4);
                int length = BitConverter.ToInt32(lenBytes.Reverse().ToArray(), 0);

                int compression = stream.ReadByte();
                byte[] compressed = new byte[length - 1];
                stream.Read(compressed, 0, compressed.Length);

                using (var ms = new MemoryStream(compressed))
                {
                    using (Stream dataStream = compression switch
                    {
                            1 => new GZipStream(ms, CompressionMode.Decompress),
                            2 => CreateZlibStream(ms),
                        }
                    )
                    { 
                        using (var outMS = new MemoryStream())
                        {
                            dataStream.CopyTo(outMS);
                            return outMS.ToArray();
                        }
                    }

                }

            }
        }

        private static Stream CreateZlibStream(Stream input)
        {
            // Skip 2-byte ZLIB header
            input.ReadByte();
            input.ReadByte();

            // DeflateStream expects RAW deflate
            return new DeflateStream(input, CompressionMode.Decompress);
        }

    }
}
