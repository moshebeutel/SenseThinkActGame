using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class Entity
    {
        public enum GAME_IDENTITY { BOT, MANUAL, UNINVOLVED, REFUGE };
        public uint Id { get; set; }
        public virtual GAME_IDENTITY GameIdentity { get; set; }
        public EntityState EntityState { get; set; }
        public bool ReadyToShoot { get; set; }
    }
}
