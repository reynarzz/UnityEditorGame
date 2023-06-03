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
        private Dictionary<World, WorldData> _worldsData;
        private WorldControllerBase _currentWorld;

        private readonly DSpriteAtlasGroup _tilesAtlas;
        private readonly PrefabInstantiator _prefabInstantiator;
        private readonly TilesDatabase _tilesDatabase;

        public WorldsManager(PrefabInstantiator prefabInstantiator, TilesDatabase tilesDatabase)
        {
            _prefabInstantiator = prefabInstantiator;
            _tilesDatabase = tilesDatabase;

            _worldsData = LoadWorldsData();

            _worlds = new Dictionary<World, WorldControllerBase>()
            {
                { World.Prologe, new PrologeWorld(_worldsData[World.Prologe], prefabInstantiator, tilesDatabase) },
                { World.Sewers, new SewersWorld(_worldsData[World.Sewers], prefabInstantiator, tilesDatabase) },
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

            if (_worlds.TryGetValue(world, out var worldController))
            {
                _currentWorld = worldController;
            }
            else
            {
                _currentWorld = new DefaultWorldController(_worldsData[world], _prefabInstantiator, _tilesDatabase);
            }

            _currentWorld.Init();

            return _currentWorld;
        }

        public void LateUpdate()
        {
            _currentWorld?.LateUpdate();
        }
    }
}
