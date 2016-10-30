using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class Player:Entity
    {
        IEnumerable<IActuator>  _actuators;
        WorldState              _currentWorldState;
        IAction                 _selectedAction;
        IHighLevelDecisionMaker _highLevelDecisionMaker;
       

        public IWorld World { get; private set; }
        public ISensor Sensor { get; private set; }

        public Player(IWorld world,
            Position position,
            ISensor sensor,
            IEnumerable<IActuator> actuators,
            IHighLevelDecisionMaker highLevelDecisionMaker)
        {
            World = world;
            Sensor = sensor;
            _actuators = actuators;
            Debug.Assert(_actuators.Any());
            _highLevelDecisionMaker = highLevelDecisionMaker;
            EntityState = new EntityState { Position = position, Strength = 1.0 };
        }

        public Player(IWorld world, EntityState state, ISensor sensor, IEnumerable<IActuator> actuators,
            IHighLevelDecisionMaker highLevelDecisionMaker ):this(world, state.Position, sensor,actuators, highLevelDecisionMaker)
        {
            EntityState.Heading = state.Heading;
            EntityState.Strength = state.Strength;
        }

        void _gameClock_Elapsed(object sender, EventArgs e)
        {
            Play();
        }

        public void Play()
        {
            Task.Factory.StartNew(SenseThinkActCycle);
        }

        private void SenseThinkActCycle()
        {
            Sense();
            Think();
            Act();
        }

        protected virtual void Act()
        {
            Debug.Assert(_selectedAction != null);
            IAction noisedSelectedAction = null;
            var selectedActuator = _actuators.FirstOrDefault(actuator =>
            {
                noisedSelectedAction =  actuator.Act(this, _selectedAction) ;
                return noisedSelectedAction != null;
            });
           // Debug.Assert(selectedActuator != null);
            // TODO deal with it
            if (selectedActuator == null)
                return;
            Debug.Assert(noisedSelectedAction != null);
            World.Act(this, noisedSelectedAction);
            _selectedAction = null;
        }

        protected virtual void Think()
        {
            Debug.Assert(_currentWorldState != null);
            //TODO inherit from player and make these sense think act virtual
            if (GameIdentity == GAME_IDENTITY.MANUAL)
            {
                // TODO velocity
                if (ReadyToShoot)
                    _selectedAction = new Shooting { AmmoType = Shooting.AMMO_TYPE.MEDIUM, Target = EntityState.Position.getOfset(2.0, EntityState.Heading) };
                else
                    _selectedAction =  new Motion { Angle = this.EntityState.Heading, Velocity = 2.0 };
                return;
            }
            _selectedAction = _highLevelDecisionMaker.GetAction(this, _currentWorldState);
            _currentWorldState = null;
        }

        protected virtual void Sense()
        {
            //TODO deal with null
            Sensor.Position = EntityState.Position;
            _currentWorldState =  World.Sense(Sensor);
        }
    }
}
