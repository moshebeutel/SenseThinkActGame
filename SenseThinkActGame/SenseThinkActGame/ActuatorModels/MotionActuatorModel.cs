using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.ActuatorModels
{
    public class MotionActuatorModel: ActuatorModel<Motion>
    {
        static public double AngularVelocity = 0.2;
        private double _angularError;
        private double _velocityError;
        private Random _random = new Random((int)DateTime.Now.Ticks);
        private double _maxVelocity;

        //TODO maxvelocity config
        public static MotionActuatorModel Perfect { get { return new MotionActuatorModel(1.0, 0.0, 0.0); } }

        public MotionActuatorModel(double maxVelocity, double velocityError, double angularError)
        {
            _velocityError = velocityError;
            _angularError = angularError;
            _maxVelocity = maxVelocity;
        }

        public override Motion GetAction(Entity entity, Motion action)
        {
            action.Velocity = Math.Min(_maxVelocity, action.Velocity);
            if (_angularError == 0.0 && _velocityError == 0.0)
                return action;
            Debug.Assert(AngularVelocity < 1 && AngularVelocity > 0);
            var standartEntityHeading = StandartAngle(entity.EntityState.Heading);
            var standartActionAngle = StandartAngle(action.Angle);
            var heading = standartEntityHeading * (1 - AngularVelocity) + AngularVelocity * standartActionAngle;
            heading = StandartAngle(heading);
            var velocity = action.Velocity * AngleVar(heading, standartActionAngle);
            var noisedVelocity = GetNoisedValue(_velocityError, velocity);
            var noisedHeading = GetNoisedValue(_angularError, heading);
            return new Motion { Velocity = noisedVelocity, Angle = noisedHeading };
        }
        double GetNoisedValue(double additiveNoiseLevel, double realValue)
        {
            return (_random.NextDouble() - 0.5) * additiveNoiseLevel + realValue;
        }

        static double AngleVar(double x, double y)
        {
             return (Math.PI - Math.Abs(x - y)) / Math.PI ; 
        }
        static double StandartAngle(double angle)
        {
            while(angle > Math.PI)
                angle -= (2 * Math.PI);
            while(angle < -Math.PI)
                angle += (2 * Math.PI);

            return angle;
        }
    }
}
