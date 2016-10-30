using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Planners.ShootingPlanners
{
    public class SimpleShootingPlanner:ActionPlanner<Shooting>
    {
        protected override IEnumerable<IAction> GetPlan(Shooting highLevelAction, Entity entity, WorldState worldState)
        {
            return new List<IAction>{new Shooting{AmmoType = highLevelAction.AmmoType, Target = entity.EntityState.Position.getOfset(1, entity.EntityState.Heading)}};
        }
    }
}
