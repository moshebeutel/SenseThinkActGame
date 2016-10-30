using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class Sensor:ISensor
    {
        public Position Position { get; set; }
        public bool IsOperational { get; set; }
        private ISensorModel _sensorModel;

        public Sensor(ISensorModel sensorModel)
        {
            _sensorModel = sensorModel;
            //TODO do something with this is operational
            IsOperational = true;
        }

        public Position GetPos(Position targetPos)
        {
            return _sensorModel.GetPos(Position, targetPos);
        }
    }
}
