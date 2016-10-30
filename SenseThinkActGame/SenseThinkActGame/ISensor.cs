using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public interface ISensor
    {
        Position Position { get; set; }
        bool IsOperational {  get; }
        //ISensorModel SensorModel { get; }
    }
}
