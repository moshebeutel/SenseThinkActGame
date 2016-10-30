using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    /// <summary>
    /// Base class for all actuators. Parametrized by the action type.
    /// Represents the actuator that physically performs the player's desired action
    /// </summary>
    /// <typeparam name="ACTION_TYPE"></typeparam>
    public abstract class Actuator<ACTION_TYPE>:IActuator where ACTION_TYPE:IAction
    {
        public Position Position { get; protected set; }
        public bool IsOperational { get; protected set; }
        public ActuatorModel<ACTION_TYPE> ActuatorModel { get; protected set; }
        public  Actuator(ActuatorModel<ACTION_TYPE> actuatorModel)
	    {
            ActuatorModel = actuatorModel;
	    }
        //TODO needs only entity state and not the entity instance
        public IAction Act(Entity entity, IAction action)
        {
            var concreteAction = action as ACTION_TYPE;
            if (concreteAction == null)
                return null;
            return Act(entity, concreteAction);
        }
        protected abstract ACTION_TYPE Act(Entity entity, ACTION_TYPE action);
    }
}
