using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame.Actions
{
    public class Shooting:IAction
    {
        public enum AMMO_TYPE { LIGHT, MEDIUM, HEAVY };
        public AMMO_TYPE AmmoType { get; set; }
        public Position Target { get; set; }
    }
}
