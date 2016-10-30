using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class World:IWorld
    {
        public WorldState RealWorldState { get; set; }
        List<Type> _supportedActionTypes = new List<Type> { typeof(Motion) };
        private IEnumerable<IWorldActionApplier> _worldAppliers;


        public World( IEnumerable<IWorldActionApplier> worldAppliers)
        {
            _worldAppliers = worldAppliers;
        }


        public void Act(Entity entity, IAction action)
        {
            Debug.Assert(RealWorldState != null);
            //TODO enable asyncronous action of various players
            var actionApplier =  _worldAppliers.FirstOrDefault(applier => applier.CanApplyOn(action));
            Debug.Assert(actionApplier != null);
            var actionValid = actionApplier.ValidateAction(entity, RealWorldState, action);
            if (!actionValid)
                throw new Exception("Action not valid");
            actionApplier.ApplyAction(entity, RealWorldState, action);
        }

        public WorldState Sense(ISensor sensor)
        {
            Debug.Assert(RealWorldState != null);
            if (!sensor.IsOperational)
                return null;
            
            var concreteSensor = sensor as Sensor;
            Debug.Assert(concreteSensor != null);

            var bot = SenseEntity(concreteSensor, RealWorldState.Bot);
            var manual = SenseEntity(concreteSensor, RealWorldState.Manual);
            var uninvolved = SenseEntities(concreteSensor, RealWorldState.Uninvolved);
            var refuges = SenseEntities(concreteSensor, RealWorldState.Refuges);

            //TODO apply fov

            return new WorldState(bot, manual, uninvolved, refuges);
        }

        static EntityState SenseEntity(Sensor sensor, EntityState entityState)
        {
            if (entityState == null)
                return null;
            var entitySensedPos = sensor.GetPos(entityState.Position);
            if (double.IsNaN(entitySensedPos.X) || double.IsNaN(entitySensedPos.Y))
                return null;
            return entityState;
        }

        static IEnumerable<EntityState> SenseEntities(Sensor sensor, IEnumerable<EntityState> entities)
        {
            var sensedEntities = new List<EntityState>();
            entities.ToList().ForEach(ent =>
            {
                var sensedEntity = SenseEntity(sensor, ent);
                if (sensedEntity != null)
                    sensedEntities.Add(sensedEntity);
            });
            return sensedEntities;
        }
    }
}
