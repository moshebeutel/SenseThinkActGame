using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Actuators
{
    public class ShootingActuator : Actuator<Shooting>
    {
        public ShootingActuator(ActuatorModel<Shooting> actuatorModel) : base(actuatorModel) { }
        protected override Shooting Act(Entity entity, Shooting action)
        {
            var noisedAction = ActuatorModel.GetAction(entity, action);
            return noisedAction;
        }
    }
}
