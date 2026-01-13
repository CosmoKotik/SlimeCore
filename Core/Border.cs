using SlimeCore.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class Border
    {
        public int[] Actions { get; set; }
        public int Diameter 
        { 
            get 
            {
                return this._diameter;
            }
            set 
            { 
                this._diameter = value;
                this.Center = new Position((value/2), 0, value/2);
            }
        }
        private int _diameter;

        public Position Center { get; set; }

        public Border(int[] actions) 
        {
            this.Actions = actions;
        }
    }
}
