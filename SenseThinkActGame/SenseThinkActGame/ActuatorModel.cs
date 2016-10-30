using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame 
{
    public abstract class ActuatorModel<ACTION_TYPE> where ACTION_TYPE:IAction
    {
        public abstract ACTION_TYPE GetAction(Entity entity, ACTION_TYPE action);
    }
}
