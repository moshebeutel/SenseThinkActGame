using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class Refuge:Entity
    {
        public enum REFUGE_TYPE { NONE, SOFT, FIRM };

        static Dictionary<REFUGE_TYPE, double> _typeToValue = new Dictionary<REFUGE_TYPE, double>
        {
            {REFUGE_TYPE.NONE, 0.0},
            {REFUGE_TYPE.SOFT, 0.5},
            {REFUGE_TYPE.FIRM, 1.0}
        };

        public override Entity.GAME_IDENTITY GameIdentity
        {
            get
            {
                return GAME_IDENTITY.REFUGE;
            }
            set
            {
                Debug.Assert(value != GAME_IDENTITY.REFUGE);
                base.GameIdentity = value;
            }
        }
       
        private REFUGE_TYPE _refugeType;

        public REFUGE_TYPE RefugeType
        {
            get { return _refugeType; }
            set { _refugeType = value; EntityState.Strength = _typeToValue[_refugeType];}
        }
        


    
    }
}
