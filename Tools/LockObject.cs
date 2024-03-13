using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Tools
{
    internal class LockObject
    {
        public object Locker { get; set; }
        public bool IsLocked { get; set; }
        public Task LockTask { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
