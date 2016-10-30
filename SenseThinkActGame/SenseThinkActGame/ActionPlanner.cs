using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public abstract class ActionPlanner<ACTION_TYPE> :IActionPlanner where ACTION_TYPE:IAction
    {
        public bool CanPlanFor(IAction highLevelAction)
        {
            return highLevelAction is ACTION_TYPE;
        }

        public  IEnumerable<IAction> GetPlan(IAction highLevelAction, Entity entity, WorldState worldState)
        {
            var concreteAction = highLevelAction as ACTION_TYPE;
            Debug.Assert(concreteAction != null);
            return GetPlan(concreteAction, entity, worldState);
        }

        protected abstract IEnumerable<IAction> GetPlan(ACTION_TYPE highLevelAction, Entity entity, WorldState worldState);
        
    }
}
