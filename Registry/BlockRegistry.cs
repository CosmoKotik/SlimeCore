using SlimeCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Registry
{
    public class BlockRegistry
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int ItemID { get; set; }
        public bool IsDefault { get; set; }
        public Direction Direction { get; set; }
        public Direction Half { get; set; }
        public bool waterlogged { get; set; }
    }

    internal class PropertiesArray
    {
        public string[] axis { get; set; }
        public string[] stage { get; set; }
        public string[] type { get; set; }
        public string[] shape { get; set; }
        public string[] up { get; set; }
        public string[] age { get; set; }
        public string[] leaves { get; set; }
        public string[] snowy { get; set; }
        public string[] occupied { get; set; }
        public string[] part { get; set; }
        public string[] distance { get; set; }
        public string[] persistent { get; set; }
        public string[] attached { get; set; }
        public string[] rotation { get; set; }
        public string[] east { get; set; }
        public string[] north { get; set; }
        public string[] south { get; set; }
        public string[] waterlogged { get; set; }
        public string[] west { get; set; }
        public string[] facing { get; set; }
        public string[] powered { get; set; }
        public string[] half { get; set; }
        public string[] hinge { get; set; }
        public string[] open { get; set; }
        public string[] in_wall { get; set; }
    }

    internal class Properties
    {
        public string axis { get; set; }
        public string stage { get; set; }
        public string type { get; set; }
        public string shape { get; set; }
        public string up { get; set; }
        public string age { get; set; }
        public string leaves { get; set; }
        public string snowy { get; set; }
        public string occupied { get; set; }
        public string part { get; set; }
        public string distance { get; set; }
        public string persistent { get; set; }
        public string attached { get; set; }
        public string rotation { get; set; }
        public string east { get; set; }
        public string north { get; set; }
        public string south { get; set; }
        public string waterlogged { get; set; }
        public string west { get; set; }
        public string face { get; set; }
        public string facing { get; set; }
        public string powered { get; set; }
        public string half { get; set; }
        public string hinge { get; set; }
        public string open { get; set; }
        public string in_wall { get; set; }
    }

    internal class States
    {
        public bool isdefault { get; set; }
        public int id { get; set; }
        public Properties properties { get; set; }
    }

    internal class registry
    {
        public PropertiesArray properties { get; set; }
        public States states { get; set; }
    }
}
