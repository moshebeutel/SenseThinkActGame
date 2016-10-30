using SenseThinkActGame.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.WorldActionAppliers
{
    public class ShootingWorldActionApplier : WorldActionApplier<Shooting>
    {
        IDictionary<Shooting.AMMO_TYPE, IEnumerable<Tuple< double, double, double>>> _ammoRangeRadiusDamagePercentage;
        IDictionary<Shooting.AMMO_TYPE, double> _maxRelevanceRadius;

        public ShootingWorldActionApplier(IDictionary<Shooting.AMMO_TYPE, IEnumerable<Tuple<double, double, double>>> ammoRangeRadiusDamagePercentage,
            IDictionary<Shooting.AMMO_TYPE, double> maxRelevanceRadius)
        {
            _ammoRangeRadiusDamagePercentage = ammoRangeRadiusDamagePercentage;
            _maxRelevanceRadius = maxRelevanceRadius;
        }


        public override bool ValidateAction(Entity entity, WorldState worldState, Shooting action)
        {
            //TODO implement
            return true;
        }

        public override void ApplyAction(Entity entity, WorldState worldState, Shooting action)
        {
            Debug.Assert(_maxRelevanceRadius.ContainsKey(action.AmmoType));
            Debug.Assert(_ammoRangeRadiusDamagePercentage.ContainsKey(action.AmmoType));
            foreach(var ent in worldState.AllEntities.Where(ent=> ent.Position.DistanceFrom(entity.EntityState.Position) > double.Epsilon 
                        && action.Target.DistanceFrom(entity.EntityState.Position) < _maxRelevanceRadius[action.AmmoType]))
            {
                var range = ent.Position.DistanceFrom(entity.EntityState.Position);
                var radius = ent.Position.DistanceFrom(action.Target);
                var rangeRadiusDamageList = _ammoRangeRadiusDamagePercentage[action.AmmoType];
                var  orderedByRangeCloserToActualRange = rangeRadiusDamageList.OrderBy(tuple => Math.Abs(tuple.Item1 - range)) ;
                var minRange = orderedByRangeCloserToActualRange.First().Item1;
                var minRangeTuplesOrderedByRadiusCloserToActual =  orderedByRangeCloserToActualRange.TakeWhile(tuple => tuple.Item1 == minRange).OrderBy(tuple => Math.Abs( tuple.Item2 - radius));
                var minRangeMinRadius = minRangeTuplesOrderedByRadiusCloserToActual.First();
                Debug.Assert(minRangeTuplesOrderedByRadiusCloserToActual.Count(tuple=>tuple.Item2 == minRangeMinRadius.Item2) == 1);
                ent.Strength *= minRangeMinRadius.Item3;
            }
        }
    }
}
