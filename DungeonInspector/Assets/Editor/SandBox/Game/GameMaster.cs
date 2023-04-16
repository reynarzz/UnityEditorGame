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
        private DTilemap _tilemap;
        public DTilemap Tilemap => _tilemap;

        public override void OnStart()
        {
            _tilemap = GetComp<DTilemap>();
        }

        private EnvironmentData GetWorldData()
        {
            var worldJson = Resources.Load<TextAsset>("World1")?.text;

            if (worldJson != null)
            {
                return JsonConvert.DeserializeObject<EnvironmentData>(worldJson);
            }
            return null;
        }
    }
}
