using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public interface  ISensorModel
    {
        Position GetPos(Position sensorPos, Position targetPos);
    }
}
