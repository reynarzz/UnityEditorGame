using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class SewersWorld : WorldControllerBase
    {
        public SewersWorld(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) : base(worldData, prefabInstantiator, tileDatabase)
        {

        }

        public override void Init()
        {
            base.Init();

            DAudio.PlayAudio("Sewers");
        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            base.OnExit();

            DAudio.StopAudio("Sewers");
        }
    }
}
