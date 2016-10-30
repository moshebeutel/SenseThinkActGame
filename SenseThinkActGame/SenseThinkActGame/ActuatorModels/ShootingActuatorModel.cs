using SenseThinkActGame.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.ActuatorModels
{
    public class ShootingActuatorModel : ActuatorModel<Shooting>
    {
        private Random _random = new Random((int)DateTime.Now.Ticks);
        private double _rangeError;
        private double _angularError;

        public static double StandartAngularError = 0.1;
        public static double StandartRangeError = 1.0;
        public static double LargeAngularError = 0.3;
        public static double LargeRangeError = 3.0;
        
        public static ShootingActuatorModel Perfect { get { return new ShootingActuatorModel(0.0, 0.0); } }
        public static ShootingActuatorModel Standart { get { return new ShootingActuatorModel(StandartAngularError, StandartRangeError); } }
        public static ShootingActuatorModel Erroneous { get { return new ShootingActuatorModel(LargeAngularError, LargeRangeError); } }

        public ShootingActuatorModel(double angularError, double rangeError)
        {
            _rangeError = rangeError;
            _angularError = angularError;
        }

        public override Shooting GetAction(Entity entity, Shooting action)
        {
            var standartEntityHeading = StandartAngle(entity.EntityState.Heading);
            var actionAngle = Math.Atan2(action.Target.Y - entity.EntityState.Position.Y, action.Target.X - entity.EntityState.Position.X);
            var standartActionAngle = StandartAngle(actionAngle);

            if (AngleVar(standartEntityHeading, standartActionAngle) > 0.5)
                return null;
            else
            {
                var noisedAngle = GetNoisedValue(_angularError, actionAngle);
                var actionRange = entity.EntityState.Position.DistanceFrom(action.Target);
                //TODO range Error should be multiplicative
                var noisedRange = GetNoisedValue(_rangeError, actionRange);
                var noisedTarget = entity.EntityState.Position.getOfset(noisedRange, noisedAngle);
                return new Shooting { Target = noisedTarget, AmmoType = action.AmmoType };
            }
        }

        //TODO move to utils
        double GetNoisedValue(double additiveNoiseLevel, double realValue)
        {
            return (_random.NextDouble() - 0.5) * additiveNoiseLevel + realValue;
        }

        //TODO move to utils
        static double StandartAngle(double angle)
        {
            while (angle > Math.PI)
                angle -= (2 * Math.PI);
            while (angle < -Math.PI)
                angle += (2 * Math.PI);

            return angle;
        }
        //TODO move to utils
        static double AngleVar(double x, double y)
        {
            return (Math.PI - Math.Abs(x - y)) / Math.PI;
        }
    }
}
