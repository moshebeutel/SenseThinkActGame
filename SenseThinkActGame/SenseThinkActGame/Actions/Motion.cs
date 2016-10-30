using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Actions
{
    public class Motion:IAction
    {
        public double Angle { get; set; }
        public double Velocity { get; set; }
    }
}
