using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class WorldState
    {
        public EntityState Bot { get; private set; }
        public EntityState Manual { get; private set; }
        public IEnumerable<EntityState> Uninvolved { get; private set; }
        public IEnumerable<EntityState> Refuges { get; private set; }

        public IEnumerable<EntityState> AllEntities { get; set; }

        public WorldState(EntityState bot ,
         EntityState manual ,
         IEnumerable<EntityState> uninvolved ,
         IEnumerable<EntityState> refuges )
        {
            Bot = bot;
            Manual = manual;
            Uninvolved = uninvolved;
            Refuges = refuges;

            var allList = new List<EntityState>();
            if(bot != null)
                allList.Add(bot);
            if(manual != null)
                allList.Add(manual);
            if(uninvolved != null && uninvolved.Any())
                allList.InsertRange(0, uninvolved);
            if(refuges != null && refuges.Any())
                allList.InsertRange(0,refuges);
      
            AllEntities = allList;
        }
    }
}
