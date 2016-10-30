using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class SensorModel:ISensorModel
    {
        public double SENSOR_SANITY_VALUE = 20;
        private double _maxSensingDist;
        private double _angularError;
        private double _positionError;
        private double _distanceError;
        private Random _random = new Random((int)DateTime.Now.Ticks);

        public static SensorModel Perfect { get {return new SensorModel(double.MaxValue, 0.0, 0.0,0.0); } }
       

        public SensorModel(double maxSensingDist, double angularError, double posError, double distanceError)
        {
            _maxSensingDist = maxSensingDist;
            _angularError = angularError;
            _positionError = posError;
            _distanceError = distanceError;
        }

        public Position GetPos(Position sensorPos, Position targetPos)
        {
            var realDistance = sensorPos.DistanceFrom(targetPos);
            if(realDistance > _maxSensingDist)
                return new Position(double.NaN, double.NaN);
            if (_angularError == 0.0 && _positionError == 0.0 && _distanceError == 0.0)
                return targetPos;
            var noisedPos = new Position(GetNoisedValue(_positionError, sensorPos.X) , GetNoisedValue(_positionError, sensorPos.Y));
            var realAngle = Math.Atan2(targetPos.Y - sensorPos.Y, targetPos.X - sensorPos.X);
            var noisedAngle = GetNoisedValue(_angularError, realAngle);
            var noisedDistance = GetNoisedValue(_distanceError, realDistance);
            var noisedTargetPos = noisedPos.getOfset(noisedDistance, noisedAngle);
            
            Debug.Assert(targetPos.DistanceFrom(noisedTargetPos) < SENSOR_SANITY_VALUE);

            return targetPos;
        }

        double GetNoisedValue(double additiveNoiseLevel, double realValue)
        {
            return (_random.NextDouble() - 0.5) * additiveNoiseLevel + realValue;
        }
    }
}
