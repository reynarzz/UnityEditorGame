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
            BackgroundMusic = "Background";
        }

        public override void Init()
        {
            base.Init();

            
        }



        public override void Update()
        {
            base.Update();



        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
