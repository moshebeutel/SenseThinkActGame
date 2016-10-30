using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.HighLevelDecisionMakers
{
    public class SimpleHighLevelDecisionMaker: IHighLevelDecisionMaker
    {
        private Position _goal;
        private IEnumerable<IActionPlanner> _actionPlanners;
        private bool _duringPlan;
        private IEnumerable<IAction> _currentExecutingPlan;

        //TODO SimpleHighLevelDecisionMaker needs goal ?
        public SimpleHighLevelDecisionMaker(Position goal, IEnumerable<IActionPlanner> actionPlanners)
	    {
            _goal = goal;
           
            _actionPlanners = actionPlanners;
	    }
        public IAction GetAction(Entity entity, WorldState state)
        {
            double angle = 0.0;
            if(_goal != null)
                 angle =  entity.EntityState.Position.AngleTo(_goal);
            //TODO implement high level decision making - subject of my thessis
            //TODO during plan ?
            //Debug.WriteLineIf(_duringPlan,"during plan");
            //TODO max velocity
            var velocity = 2.0; 
            IAction decidedAction  = null;
            if (entity.ReadyToShoot)
                decidedAction = new Shooting { AmmoType = Shooting.AMMO_TYPE.MEDIUM, Target = entity.GameIdentity == Entity.GAME_IDENTITY.MANUAL ? state.Bot.Position : state.Manual.Position };
            else
                decidedAction  = new Motion { Angle = angle, Velocity = velocity };

            var actionPlannerForDecidedAction = _actionPlanners.FirstOrDefault(actionPlanner => actionPlanner.CanPlanFor(decidedAction));
            Debug.Assert(actionPlannerForDecidedAction != null);
            var plan = actionPlannerForDecidedAction.GetPlan(decidedAction, entity, state);
            Debug.Assert(plan != null && plan.Any());
            _currentExecutingPlan = plan;
            _duringPlan = true;
            return plan.First();
        }
    }
}
