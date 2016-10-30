using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Planners.MotionPlanners
{
    public class TargetedMotionPlanner: ActionPlanner<Motion>
    {
        private Position _goal;
        private double _turningResolution;
        private uint NUM_OF_TURNS = 4;

        public TargetedMotionPlanner(Position goal, double turningResolution)
        {
            _goal = goal;
            _turningResolution = turningResolution;

        }
        protected override IEnumerable<IAction> GetPlan(Motion highLevelAction, Entity entity, WorldState worldState)
        {
            var plan = new List<IAction>();
            var pos = entity.EntityState.Position;
            var heading = entity.EntityState.Position.AngleTo(_goal);
            //TODO config this factor of medium level control
            var goal = entity.EntityState.Position.getOfset(10 * highLevelAction.Velocity, heading);
          
            //TODO think about this 5
            while(!IsWayOpenFromPosToTargetPosition(pos, worldState, goal) && plan.Count() < 5)
            {
                var turning = true;
                var turningIndex = 0u;
                Position midPoint = null;
                while(turning && (++turningIndex) < NUM_OF_TURNS)
                {
                    var newHeading = heading + turningIndex * _turningResolution;
                    midPoint = pos.getOfset(2 * highLevelAction.Velocity, newHeading);
                    turning =  !IsWayOpenFromPosToTargetPosition(pos, worldState, midPoint);
 
                }
                if(turningIndex >= NUM_OF_TURNS)
                    return plan;
                plan.Add(new Motion { Angle = pos.AngleTo(midPoint), Velocity = highLevelAction.Velocity });

            }
            plan.Add(new Motion{Angle = pos.AngleTo(_goal), Velocity = highLevelAction.Velocity});
            return plan;
        }

        static bool IsWayOpenFromPosToTargetPosition(Position pos, WorldState worldState, Position targetPosition)
        {
            return worldState.AllEntities.All(ent => ent.Position.DistanceFrom(pos) < Double.Epsilon || !ent.Position.IsBetween(pos, targetPosition));
        }
    }
}
