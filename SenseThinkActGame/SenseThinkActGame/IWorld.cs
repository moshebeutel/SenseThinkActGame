using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public interface IWorld
    {
        WorldState Sense(ISensor Sensor);
        void Act(Entity entity, IAction action);
    }
}
