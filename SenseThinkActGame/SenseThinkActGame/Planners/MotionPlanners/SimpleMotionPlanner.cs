using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Planners.MotionPlanners
{
    public class SimpleMotionPlanner:ActionPlanner<Motion>
    {
        protected override IEnumerable<IAction> GetPlan(Motion highLevelAction, Entity entity, WorldState worldState)
        {
            return worldState.AllEntities.All(ent => ent.Position.DistanceFrom(entity.EntityState.Position) > double.Epsilon || ent.Position.IsBetween(entity.EntityState.Position, entity.EntityState.Position.getOfset(highLevelAction.Velocity, highLevelAction.Angle))) ?
                new List<IAction>{highLevelAction} : null;
        }
    }
}
