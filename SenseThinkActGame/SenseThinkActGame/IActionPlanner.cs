using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActionPlanner
    {
        bool CanPlanFor(IAction highLevelAction);
        IEnumerable<IAction> GetPlan(IAction highLevelAction, Entity entity, WorldState worldState);
    }
}
