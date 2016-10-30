using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.HighLevelDecisionMakers
{
    public class HighLevelDecisionMaker: IHighLevelDecisionMaker
    {//TODO inherit from simpleHighLevelDecisionMaker  ?
        private uint _numberOfMovesForwardLooking;
        private Game _offlineGame;
        private GameClock _gameClock;
        private Position _goal;
        private IEnumerable<IActionPlanner> _actionPlanners;
        private bool _duringPlan;
        private IEnumerable<IAction> _currentExecutingPlan;
        private IPlayerFactory _playerFactory;

        public HighLevelDecisionMaker(Position goal, Game offlineGame, GameClock gameClock,  IEnumerable<IActionPlanner> actionPlanners, IPlayerFactory playerFactory ,uint numberOfMovesForwardLooking = 0)
        {
            _goal = goal;
            _numberOfMovesForwardLooking = numberOfMovesForwardLooking;
            _offlineGame = offlineGame;
            _gameClock = gameClock;
            _actionPlanners = actionPlanners;
            _playerFactory = playerFactory;
        }

        public IAction GetAction(Entity entity, WorldState state)
        {
            UpdateOfflineGame(state);

            for (var move = 0u; move < _numberOfMovesForwardLooking; move++)
            {
                MakeAnOfflineMove();
            }
            //TODO choose an action using heuristics on game states
            double angle = 0.0;
            if (_goal != null)
                angle = entity.EntityState.Position.AngleTo(_goal);

            var velocity = 2.0;
            IAction decidedAction = new Motion { Angle = angle, Velocity = velocity };
            var actionPlannerForDecidedAction = _actionPlanners.FirstOrDefault(actionPlanner => actionPlanner.CanPlanFor(decidedAction));
            Debug.Assert(actionPlannerForDecidedAction != null);
            var plan = actionPlannerForDecidedAction.GetPlan(decidedAction, entity, state);
            Debug.Assert(plan != null && plan.Any());
            _currentExecutingPlan = plan;
            _duringPlan = true;
            return plan.First();
            
            
        }

        private void UpdateOfflineGame(WorldState state)
        {
            var id = 0u;
            _offlineGame.Bot = _playerFactory.CreatePlayer(new Entity() { GameIdentity = Entity.GAME_IDENTITY.BOT, EntityState = state.Bot, Id = ++id });
            //TODO manual default state
            if(state.Manual != null)
                _offlineGame.Manual = _playerFactory.CreatePlayer(new Entity() { GameIdentity = Entity.GAME_IDENTITY.MANUAL, EntityState = state.Manual, Id = ++id });
            state.Uninvolved.ToList().ForEach(uninvolvedState => _offlineGame.RefugeList.Add(_playerFactory.CreatePlayer(new Entity() { GameIdentity = Entity.GAME_IDENTITY.UNINVOLVED, EntityState = uninvolvedState, Id = ++id })));
            //TODO refuge types
            state.Refuges.ToList().ForEach(refugeState => _offlineGame.RefugeList.Add(new Refuge() { EntityState = refugeState, Id = ++id }));
        }

        private void MakeAnOfflineMove()
        {
            _gameClock.Advance();
            //TODO move manual randomly
            //TODO how to choose manual moves offline
            // 1. Take what he has done in the last n moves as a distribution and pick from it.
            // 2. Continue his last action.
            //TODO record manual moves and perform those same moves against various bot moves
         //   if(_offlineGame.Manual != null)
         //       _offlineGame.MoveManual(SenseThinkActGame.Game.DIRECTION.LEFT);
        }

    }
}
