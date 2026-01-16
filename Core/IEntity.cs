using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public interface IEntity
    {
        public int EntityID { get; set; }
        public string Username { get; set; }
    }
}
