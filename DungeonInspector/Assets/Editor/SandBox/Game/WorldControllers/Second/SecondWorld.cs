using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class SecondWorld : WorldControllerBase
    {
        public SecondWorld(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) 
            : base(worldData, prefabInstantiator, tileDatabase)
        {
        }
    }
}
