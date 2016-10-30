using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class EventArgs<DATA_TYPE> : EventArgs
    {
        public DATA_TYPE Data { get; private set; }
        public EventArgs(DATA_TYPE data)
        {
            Data = data;
        }
    }
}
