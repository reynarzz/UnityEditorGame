using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class PrologeWorld : WorldControllerBase
    {
        public PrologeWorld(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) : 
            base(worldData, prefabInstantiator, tileDatabase)
        {

        }

        public override void Init()
        {
            base.Init();

            DAudio.PlayAudio("Background");
        }

        public override void OnExit()
        {
            base.OnExit();

            DAudio.StopAudio("Background");
        }
    }
}
