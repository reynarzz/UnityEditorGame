using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public class DefaultWorldController : WorldControllerBase
    {
        public DefaultWorldController(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) : 
            base(worldData, prefabInstantiator, tileDatabase)
        {
        }

        public override void Init()
        {
            base.Init();
            Debug.Log("Using default world controller!");
        }
    }
}
