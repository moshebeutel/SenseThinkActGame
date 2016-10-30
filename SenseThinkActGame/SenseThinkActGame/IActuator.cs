using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public interface IActuator
    {
        Position Position { get; }
        bool IsOperational { get; }
        IAction Act(Entity entity, IAction action);
    }
}
