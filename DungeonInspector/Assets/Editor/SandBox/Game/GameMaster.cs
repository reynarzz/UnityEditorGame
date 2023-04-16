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
        private DCamera _camera;

        public DTilemap Tilemap => _tilemap;
        public DCamera Camera => _camera;

        private TilesDatabase _tilesDatabase;
        public TilesDatabase TilesDatabase => _tilesDatabase;

        public override void OnStart()
        {
            _tilemap = FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase();
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
