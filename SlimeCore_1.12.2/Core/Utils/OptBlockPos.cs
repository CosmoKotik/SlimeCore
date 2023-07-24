using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Utils
{
    public class OptBlockPos
    {
        public bool IsPresent = false;
        public Vector3 OptionalVector3 = Vector3.zero;

        public static OptBlockPos Empty = new OptBlockPos();
    }
}
