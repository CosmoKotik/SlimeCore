using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Enums
{
    public class Difficulty
    {
        public enum Difficulties : byte
        { 
            peaceful = 0,
            easy = 1,
            normal = 2,
            hard = 3
        }
    }
}
