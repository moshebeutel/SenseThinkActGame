using SenseThinkActGame.ActuatorModels;
using SenseThinkActGame.Actuators;
using SenseThinkActGame.HighLevelDecisionMakers;
using SenseThinkActGame.Planners.MotionPlanners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public  class PlayerFactory:IPlayerFactory
    {
        private IWorld _world;

        static Dictionary<Entity.GAME_IDENTITY, Func<IEnumerable<IActionPlanner>>> GetPlannersDic = new Dictionary<Entity.GAME_IDENTITY,Func<IEnumerable<IActionPlanner>>>
        {
            {Entity.GAME_IDENTITY.BOT, GetPlannersForBot},
            {Entity.GAME_IDENTITY.MANUAL, GetPlannersForManual},
            {Entity.GAME_IDENTITY.UNINVOLVED, GetPlannersForUninvolved}
        };

        static Dictionary<Entity.GAME_IDENTITY, Func<IEnumerable<IActuator>>> GetActuatorsDic = new Dictionary<Entity.GAME_IDENTITY, Func<IEnumerable<IActuator>>>
        {
            {Entity.GAME_IDENTITY.BOT, GetActuatorsForBot},
            {Entity.GAME_IDENTITY.MANUAL, GetActuatorsForManual},
            {Entity.GAME_IDENTITY.UNINVOLVED, GetActuatorsForUninvolved}
        };

        public PlayerFactory(IWorld world)
        {
            _world = world;
        }

        public Player CreatePlayer(Entity entity)
        {
            var sensor = new Sensor(new SensorModel(Game.SensingDistance, 0.0, 0.0, 0.0));
            var adm = new SimpleHighLevelDecisionMaker(null, GetPlanners(entity.GameIdentity));
            return new Player(_world, entity.EntityState, sensor, GetActuators(entity.GameIdentity), adm) { GameIdentity = entity.GameIdentity, Id = entity.Id };
        }

        static IEnumerable<IActionPlanner> GetPlanners(Entity.GAME_IDENTITY identity )
        {
            return GetPlannersDic[identity]();
        }

        static IEnumerable<IActionPlanner> GetPlannersForBot()
        {
            return new List<IActionPlanner> { new SimpleMotionPlanner() };
        }
        static IEnumerable<IActionPlanner> GetPlannersForManual()
        {
            return new List<IActionPlanner> { new SimpleMotionPlanner() };
        }
        static IEnumerable<IActionPlanner> GetPlannersForUninvolved()
        {
            return new List<IActionPlanner>{new RandomMotionPlanner()};
        }

        static IEnumerable<IActuator> GetActuators(Entity.GAME_IDENTITY identity)
        {
            return GetActuatorsDic[identity]();
        }

        static IEnumerable<IActuator> GetActuatorsForBot()
        {
            return new List<IActuator> { new MotionActuator(MotionActuatorModel.Perfect), new ShootingActuator(ShootingActuatorModel.Standart) };
        }
        static IEnumerable<IActuator> GetActuatorsForManual()
        {
            return new List<IActuator> { new MotionActuator(MotionActuatorModel.Perfect), new ShootingActuator(ShootingActuatorModel.Standart) };
        }
        static IEnumerable<IActuator> GetActuatorsForUninvolved()
        {
            return new List<IActuator> { new MotionActuator(MotionActuatorModel.Perfect) };
        }
        
    }
}
