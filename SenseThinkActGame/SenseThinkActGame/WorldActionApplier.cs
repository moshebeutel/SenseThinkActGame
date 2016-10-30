using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public abstract class WorldActionApplier<T> : IWorldActionApplier where T : IAction
    {
       
        public abstract bool ValidateAction(Entity entity, WorldState worldState, T action);
        public abstract void ApplyAction(Entity entity, WorldState worldState, T action);

        public bool CanApplyOn(IAction action)
        {
            return action is T;
        }

        public bool ValidateAction(Entity entity, WorldState worldState, IAction action)
        {
            var concreteAction = action as T;
            Debug.Assert(concreteAction != null);
            return ValidateAction(entity,worldState, concreteAction);
        }

        public void ApplyAction(Entity entity, WorldState worldState, IAction action)
        {
            var concreteAction = action as T;
            Debug.Assert(concreteAction != null);
            ApplyAction(entity, worldState, concreteAction);
        }
    }
}
