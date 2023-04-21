using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DungeonInspector
{
    public class GameMaster : DBehavior
    {
        private DTilemap _tilemap;
        private DCamera _camera;

        public DTilemap Tilemap => _tilemap;
        public DCamera Camera => _camera;

        private TilesDatabase _tilesDatabase;
        private TilesDatabase _animatedTiles;
        private EnemyDatabase _enemyDatabase;

        public TilesDatabase TilesDatabase => _tilesDatabase;
        public TilesDatabase AnimatedTiles => _animatedTiles;
        public EnemyDatabase EnemyDatabase => _enemyDatabase;

        private PrefabInstantiator _prefabInstantiator;


        private TileBehaviorsContainer _tbContainer;

        private LevelData _levelData;

        private Dictionary<TileBehavior, List<Actor>> _tilesBehaviors;
        protected override void OnAwake()
        {
            _tilemap = DGameEntity.FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _enemyDatabase = new EnemyDatabase();
            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Actor>>();
            _prefabInstantiator = new PrefabInstantiator();


            var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

            if (File.Exists(worldLevelPath))
            {
                var json = File.ReadAllText(worldLevelPath);

                _levelData = JsonConvert.DeserializeObject<LevelData>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            }
            else
            {
                _levelData = new LevelData(new TileData[0]);
            }

            _prefabInstantiator.InstanceDoor("ExitDoor").Transform.Position = new DVector2(2.27f, 3.56f);
            _prefabInstantiator.InstanceCollectible<HealthPotion>("Health1").Transform.Position = new DVector2(2, 1);
        }

        protected override void OnStart()
        {
            //DAudio.PlayAudio("Audio/ForgottenPlains/Music/Plain_Sight_(Regular).wav");

            Load();
        }

        private void Load()
        {
            for (int i = 0; i < _levelData.Count; i++)
            {
                var info = _levelData.GetTile(i);

                _tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
            }
        }


        protected override void OnUpdate()
        {
            Utils.DrawBounds(_tilemap.GetTilemapBoundaries(), Color.white);

            UpdateTilesBehavior();
        }

        private void UpdateTilesBehavior()
        {
            foreach (var item in _tilesBehaviors)
            {
                // This should be cached
                var behavior = _tbContainer.GetBehavior(item.Key);

                for (int i = 0; i < item.Value.Count; i++)
                {
                    behavior.OnUpdate(item.Value[i], GetLevelData(item.Value[i].Transform.Position));
                }
            }
        }

        public void OnActorEnterTile(Actor actor, DTile tile)
        {
            var behavior = _tbContainer.GetBehavior(tile.Behavior);

            behavior.OnEnter(actor, GetLevelData(actor.Transform.Position));

            if (_tilesBehaviors.TryGetValue(tile.Behavior, out var playersList))
            {
                if (!playersList.Contains(actor))
                {
                    playersList.Add(actor);
                }
            }
            else
            {
                _tilesBehaviors.Add(tile.Behavior, new List<Actor>() { actor });
            }
        }

        public void OnActorExitTile(Actor actor, DTile tile)
        {
            var behavior = _tbContainer.GetBehavior(tile.Behavior);

            behavior.OnExit(actor, GetLevelData(actor.Transform.Position));

            if (_tilesBehaviors.TryGetValue(tile.Behavior, out var playersList))
            {
                playersList.Remove(actor);

                if (playersList.Count == 0)
                {
                    _tilesBehaviors.Remove(tile.Behavior);
                }
            }
        }

        public BaseTD GetLevelData(DVector2 vector)
        {
            return _levelData.GetLevelTileData(vector.Round());
        }
    }
}