using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Planners.ShootingPlanners
{
    public class ShootEnemyPlanner:ActionPlanner<Shooting>
    {
        private Entity _enemy;
        private bool _uninvolvedSafty;
        public ShootEnemyPlanner(Entity enemy, bool uninvolvedSafty)
        {
            _enemy = enemy;
            _uninvolvedSafty = uninvolvedSafty;
        }
        protected override IEnumerable<IAction> GetPlan(Shooting highLevelAction, Entity entity, WorldState worldState)
        {
            var targetPos = _enemy.EntityState.Position;
            var sourcePos = entity.EntityState.Position;
            Debug.Assert(entity.Id != _enemy.Id);
            Debug.Assert(targetPos.DistanceFrom(sourcePos) > double.Epsilon);
            var isLineOfFireEmpty = _uninvolvedSafty && worldState.AllEntities.All(ent => ent.Position.DistanceFrom(entity.EntityState.Position) > double.Epsilon || ent.Position.DistanceFrom(_enemy.EntityState.Position) < double.Epsilon || !ent.Position.IsBetween(sourcePos, targetPos));
            return isLineOfFireEmpty ? new List<IAction> { new Shooting { AmmoType = highLevelAction.AmmoType, Target = _enemy.EntityState.Position } } : null;
        }
    }
}
