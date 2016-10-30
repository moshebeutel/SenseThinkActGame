using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Planners.MotionPlanners
{
    public class RandomMotionPlanner: ActionPlanner<Motion>
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        protected override IEnumerable<IAction> GetPlan(Motion highLevelMotion, Entity entity, WorldState worldState)
        {
            var found = false;
            var debugCounter = 0;
            var angle = highLevelMotion.Angle;
            while(!found && ++debugCounter < 100)
            {
                angle +=  (_random.NextDouble() - 0.5) * 2.0 * Math.PI;
                var goal = entity.EntityState.Position.getOfset(highLevelMotion.Velocity, angle);
                found = worldState.AllEntities.All(ent => ent.Position.DistanceFrom(entity.EntityState.Position) < double.Epsilon || !ent.Position.IsBetween(entity.EntityState.Position, goal));
            }
            Debug.Assert(debugCounter < 100);        
            return new List<IAction> { new Motion { Angle = angle, Velocity = highLevelMotion.Velocity } };
        }
    }
}
