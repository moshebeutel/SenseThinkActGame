using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.WorldActionAppliers
{
    public class MotionWorldActionApplier : WorldActionApplier<Motion>
    {
        private double _minDistanceToEntity;
        public MotionWorldActionApplier(double minDistanceToEntity)
        {
            _minDistanceToEntity = minDistanceToEntity;
        }

        public override bool ValidateAction(Entity entity, WorldState worldState, Motion action)
        {
            var desiredPos = entity.EntityState.Position.getOfset(action.Velocity, action.Angle);
            return worldState.AllEntities.All(ent => ent.Position.DistanceFrom(entity.EntityState.Position) > double.Epsilon || ent.Position.DistanceFrom(desiredPos) > _minDistanceToEntity);
        }

        public override void ApplyAction(Entity entity, WorldState worldState, Motion action)
        {
            entity.EntityState.Position = entity.EntityState.Position.getOfset(action.Velocity, action.Angle);
        }
    }
}
