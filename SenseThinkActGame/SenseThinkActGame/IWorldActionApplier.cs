using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public interface IWorldActionApplier
    {
        bool CanApplyOn(IAction action);
        bool ValidateAction(Entity entity, WorldState worldState, IAction action);
        void ApplyAction(Entity entity, WorldState worldState, IAction action);
    }
}
