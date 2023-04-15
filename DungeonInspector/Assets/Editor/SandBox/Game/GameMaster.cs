using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DGameMaster : DBehavior
    {
        private WorldData _currentWorldData;
        public WorldData CurrentWorldData => GetWorldData();

        public override void OnStart()
        {
          
        }

        private WorldData GetWorldData()
        {
            var worldJson = Resources.Load<TextAsset>("World1")?.text;

            if (worldJson != null)
            {
                return JsonConvert.DeserializeObject<WorldData>(worldJson);
            }
            return null;
        }

    }
}
