using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public abstract class WorldControllerBase
    {
        private DTilemap _tilemap;
        private DTilemap[] _tilemaps;
        private readonly PrefabInstantiator _prefabInstantiator;
        private readonly TilesDatabase _tilesDatabase;

        private NavWorld _navWorld;
        private TilemapArena _tilemapArena;
        private readonly WorldData _worldData;
        public TilemapData[] TilemapsData => _worldData.TilemapsData;
        public NavWorld NavWorld => _navWorld;
        public DTilemap Tilemap => _tilemap;
        private DGameEntity[] _entities;
        protected string BackgroundMusic { get; set; }

        public Player Player { get; private set; }
        //public TilemapArena Tilemap => _tilemapArena;

        public WorldControllerBase(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase)
        {
            _worldData = worldData;
            _prefabInstantiator = prefabInstantiator;
            _tilesDatabase = tileDatabase;
        }

        public virtual void Update() { }

        public virtual void Init()
        {
            DAudio.PlayAudio(BackgroundMusic);

            Player = DGameEntity.FindGameEntity("Player").GetComp<Player>();

            _tilemaps = new DTilemap[_worldData.TilemapsData.Length];

            for (int i = 0; i < _worldData.TilemapsData.Length; i++)
            {
                var tilemap = GetNewTilemap("Tilemap: " + i, _worldData.TilemapsData.Length - i - 2);

                for (int j = 0; j < _worldData.TilemapsData[i].Count; j++)
                {
                    var info = _worldData.TilemapsData[i].GetTile(j);

                    tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
                }

                _tilemaps[i] = tilemap;
            }

            // Set the first one as the main, (TODO: make a tilemap manager to calculate the current one, and get walkable states)
            _tilemap = _tilemaps.Last();
            _navWorld = new NavWorld(_tilemap);

            _entities = new DGameEntity[_worldData.Entities.Count];

            for (int i = 0; i < _worldData.Entities.Count; i++)
            {
                var ent = _worldData.Entities[i];

                var obj = _prefabInstantiator.InstanceEntityByID(ent.Item1);

                _entities[i] = obj;

                if (obj != null)
                {
                    obj.Transform.Position = ent.Item2;
                }

                if (ent.Item1 == EntityID.ChestEmpty)
                {
                    var tile = _tilemap.GetTile(ent.Item2);

                    if (tile != null)
                    {
                        tile.IsWalkable = false;
                    }
                    else
                    {
                        Debug.Log("Placed outside a tile.");
                    }
                }
            }

            _navWorld.Init();
        }

        protected DGameEntity FindWorldEntity(string name)
        {
            for (int i = 0; i < _entities.Length; i++)
            {
                var entity = _entities[i];

                if (entity.Name.Equals(name))
                {
                    return entity;
                }
            }

            Debug.LogError($"Object '{name}' doesn't exist.");

            return null;
        }

        protected T[] FindWorldEntitiesOfType<T>() where T : DBehavior, new()
        {
            var entities = new List<T>();

            for (int i = 0; i < _entities.Length; i++)
            {
                var entity = _entities[i];

                if (entity.TryGetComponent<T>(out var component))
                {
                    entities.Add(component);
                }
            }

            return entities.ToArray();
        }


        private DTilemap GetNewTilemap(string name, int sorting)
        {
            var tilemapObj = new DGameEntity(name);
            var tilemap = tilemapObj.AddComp<DTilemap>();

            var renderer = tilemapObj.AddComp<DTilemapRendererComponent>();
            renderer.TileMap = tilemap;
            renderer.ZSorting = sorting;
            return tilemap;
        }

        public virtual void OnExit() 
        {
            _prefabInstantiator.DestroyAllInstances();

            if (_tilemaps != null)
            {
                for (int i = 0; i < _tilemaps.Length; i++)
                {
                    _tilemaps[i].Entity.Destroy();
                }

                _tilemaps = null;
            }

            _entities = null;

            DAudio.StopAudio(BackgroundMusic);
        }

        public virtual void LateUpdate()
        {
            _navWorld.OnLateUpdate();
        }
    }
}
