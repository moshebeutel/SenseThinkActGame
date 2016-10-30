using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Actuators
{
    public class MotionActuator:Actuator<Motion>
    {
        public MotionActuator(ActuatorModel<Motion> actuatorModel):base(actuatorModel)
        {

        }
        protected override Motion Act(Entity entity, Motion action)
        {
            var noisedAction = ActuatorModel.GetAction(entity,action);
            return noisedAction;    
        }
    }
}
