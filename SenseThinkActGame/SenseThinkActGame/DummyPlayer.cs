using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class DummyPlayer:Player
    {
        public DummyPlayer(IWorld world, Position position):base(world, position, null, null, null)
        {

        }
    }
}
