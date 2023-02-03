using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    public class Vector3
    {
        public long x;
        public long y;
        public long z;

        public static Vector3 zero = new Vector3(0, 0, 0);

        public Vector3(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
