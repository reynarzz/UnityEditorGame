using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public enum World
    {
        Prologe,
        Sewers,
        Second,
        Two,
        Final,
    }

    public class WorldsManager
    {
        private Dictionary<World, WorldControllerBase> _worlds;
        private WorldControllerBase _currentWorld;

        public WorldsManager(PrefabInstantiator prefabInstantiator, TilesDatabase tilesDatabase)
        {
            var worldData = LoadWorldsData();

            _worlds = new Dictionary<World, WorldControllerBase>()
            {
                { World.Prologe, new PrologeWorld(worldData[World.Prologe], prefabInstantiator, tilesDatabase) },
                { World.Sewers, new SewersWorld(worldData[World.Sewers], prefabInstantiator, tilesDatabase) },
            };
        }

        private Dictionary<World, WorldData> LoadWorldsData()
        {
            var worldsData = new Dictionary<World, WorldData>();
            var worldLevelPath = Application.dataPath + "/Resources/Data/WorldData.txt";

            if (File.Exists(worldLevelPath))
            {
                var json = File.ReadAllText(worldLevelPath);

                var worldData = JsonConvert.DeserializeObject<List<WorldData>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                for (int i = 0; i < worldData.Count; i++)
                {
                    worldsData.Add(worldData[i].World, worldData[i]);
                }
            }

            return worldsData;
        }

        public void Update()
        {
            _currentWorld?.Update();
        }

        public WorldControllerBase Load(World world)
        {
            if (_currentWorld != null)
            {
                _currentWorld.OnExit();
            }

            _currentWorld = _worlds[world];

            _currentWorld.Init();

            return _currentWorld;
        }

        public void LateUpdate()
        {
            _currentWorld?.LateUpdate();
        }
    }
}
