using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class EntityState
    {
        public virtual Position Position { get; set; }
        public virtual double Heading { get; set; }
        public virtual double Strength { get; set; }
    }
}
