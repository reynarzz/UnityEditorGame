using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

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
        private PlayerHealthUI _playerHealthUI;

        public PrefabInstantiator PrefabInstantiator => _prefabInstantiator;

        private TileBehaviorsContainer _tbContainer;

        private LevelTilesData _levelData;

        private Dictionary<TileBehavior, List<Actor>> _tilesBehaviors;
        private NavWorld _navWorld;
        public NavWorld NavWorld => _navWorld;
        private Player _player;
        private ScreenUI _screenUI;
        private Dictionary<string, WorldData> _worldData;

        protected override void OnAwake()
        {
            // Main Player
            _player = new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player)).GetComp<Player>();

            _tilemap = DGameEntity.FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _enemyDatabase = new EnemyDatabase();
            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Actor>>();
            _prefabInstantiator = new PrefabInstantiator();

            _navWorld = new NavWorld(_tilemap);
            _screenUI = new DGameEntity("ScreenUI", typeof(DRendererUIComponent), typeof(ScreenUI)).GetComp<ScreenUI>();

            var worldLevelPath = Application.dataPath + "/Resources/Data/WorldData.txt";

            _worldData = new Dictionary<string, WorldData>();

            if (File.Exists(worldLevelPath))
            {
                var json = File.ReadAllText(worldLevelPath);

                var worldData = JsonConvert.DeserializeObject<List<WorldData>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                for (int i = 0; i < worldData.Count; i++)
                {
                    _worldData.Add(worldData[i].Name, worldData[i]);
                }

                _levelData = worldData[0].LevelData;
            }
            else
            {
                _levelData = new LevelTilesData(new TileData[0]);
            }

            _playerHealthUI = new DGameEntity("PlayerHealthHUD").AddComp<PlayerHealthUI>();

            //_prefabInstantiator.InstanceDoor("ExitDoor").Transform.Position = new DVec2(2.27f, 3.56f);
            //_prefabInstantiator.GetHealthPotion("Health1").Transform.Position = new DVec2(2, 1);
            _prefabInstantiator.InstanceCoin("Coin1").Transform.Position = new DVec2(-1, -4);
            //_prefabInstantiator.InstanceCoin("Coin2").Transform.Position = new DVec2(2, 2);
            //_prefabInstantiator.InstanceCoin("Coin3").Transform.Position = new DVec2(3, 2);
            //_prefabInstantiator.InstanceCoin("Coin4").Transform.Position = new DVec2(4, 2);
            //_prefabInstantiator.InstanceCoin("Coin5").Transform.Position = new DVec2(5, 2);
            //_prefabInstantiator.InstanceCoin("Coin6").Transform.Position = new DVec2(6, 2);

            Load();
            _navWorld.Init();
        }

        protected override void OnStart()
        {
            //DAudio.PlayAudio("Audio/ForgottenPlains/Music/Plain_Sight_(Regular).wav");

            var chest = _prefabInstantiator.InstanceChest("Chest");
            chest.Transform.Position = new DVec2(-5, 2);

            _tilemap.GetTile(new DVec2(-5, 2), 0).IsWalkable = false;
        }

        private void Load()
        {
            _tilemap.Clear();

            for (int i = 0; i < _levelData.Count; i++)
            {
                var info = _levelData.GetTile(i);

                _tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
            }
        }

        public void ChangeToLevel(string name)
        {
            if (_worldData.TryGetValue(name, out var world))
            {
                _levelData = world.LevelData;
                Load();
            }
            else
            {
                Debug.Log("Can't load level!: No level with name: '" + name + "' exist");
            }
        }

        protected override void OnUpdate()
        {
            //-Utils.DrawBounds(_tilemap.GetTilemapBoundaries(), Color.white, 0.5f);

            UpdateTilesBehavior();

            EditorGUILayout.Space(18);

        }

        protected override void OnLateUpdate()
        {
            _navWorld.OnLateUpdate();
        }

        private void UpdateTilesBehavior()
        {
            //foreach (var actorsList in _tilesBehaviors)
            //{
            //    // This should be cached
            //    var behavior = _tbContainer.GetBehavior(actorsList.Key);

            //    for (int i = 0; i < actorsList.Value.Count; i++)
            //    {
            //        behavior.OnUpdate(actorsList.Value[i], GetLevelData(actorsList.Value[i].Transform.Position));
            //    }
            //}
        }

        public void OnActorEnterTile(Actor actor, DTile tile)
        {
            var behavior = _tbContainer.GetBehavior(tile.Behavior);

            var tileData = GetLevelData(actor.Transform.Position);

            behavior.OnEnter(actor, tileData);

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

        public BaseTD GetLevelData(DVec2 vector)
        {
            return _levelData.GetLevelTileData(vector.Round());
        }
    }
}