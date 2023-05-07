using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

        public TilesDatabase TilesDatabase => _tilesDatabase;
        public TilesDatabase AnimatedTiles => _animatedTiles;

        private PrefabInstantiator _prefabInstantiator;
        private PlayerHealthUI _playerHealthUI;

        public PrefabInstantiator PrefabInstantiator => _prefabInstantiator;

        private TileBehaviorsContainer _tbContainer;

        private Dictionary<TileBehavior, List<Actor>> _tilesBehaviors;
        private NavWorld _navWorld;
        public NavWorld NavWorld => _navWorld;
        private Player _player;
        private ScreenUI _screenUI;
        private Dictionary<string, WorldData> _worldsData;
        private WorldData _currentWorld;

        private GameInput _input;
        public GameInput Input => _input;
        private Texture2D _cursorTex;
        private DRendererAtlasComponent _cursor;

        protected override void OnAwake()
        {
            DTime.TimeScale = 1;
            // Main Player
            _player = new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player)).GetComp<Player>();

            _tilemap = DGameEntity.FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Actor>>();
            _prefabInstantiator = new PrefabInstantiator();

            _navWorld = new NavWorld(_tilemap);
            _screenUI = new DGameEntity("ScreenUI", typeof(DRendererUIComponent), typeof(ScreenUI)).GetComp<ScreenUI>();

            var worldLevelPath = Application.dataPath + "/Resources/Data/WorldData.txt";

            _worldsData = new Dictionary<string, WorldData>();

            var currentWorld = default(WorldData);

            if (File.Exists(worldLevelPath))
            {
                var json = File.ReadAllText(worldLevelPath);

                var worldData = JsonConvert.DeserializeObject<List<WorldData>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                for (int i = 0; i < worldData.Count; i++)
                {
                    _worldsData.Add(worldData[i].Name, worldData[i]);
                }

                currentWorld = worldData[0];
            }

            _playerHealthUI = new DGameEntity("PlayerHealthHUD").AddComp<PlayerHealthUI>();
            _cursorTex = Resources.Load<Texture2D>("GameAssets/GUI/GunSights");

            _cursor = new DGameEntity("Cursor").AddComp<DRendererAtlasComponent>();

            _cursor.AtlasInfo.Texture = _cursorTex;
            _cursor.AtlasInfo.BlockSIze = 5;
            _cursor.SpriteCoord = new DVec2(2, 1);
            _cursor.ZSorting = 30;
            _cursor.Transform.Scale = new DVec2(0.43f, 0.42f);

            if (currentWorld != null)
            {
                Load(currentWorld);
                
            }
        }

        private void OnPlayerDead()
        {
            ChangeToLevel(_currentWorld.Name);
        }

        protected override void OnStart()
        {
            //DAudio.PlayAudio("Audio/ForgottenPlains/Music/Plain_Sight_(Regular).wav");

            _player.GetComp<ActorHealth>().OnHealthDepleted += OnPlayerDead;
        }

        private void Load(WorldData world)
        {
            _tilemap.Clear();

            _prefabInstantiator.DestroyAllInstances();

            _currentWorld = world;

            for (int i = 0; i < world.LevelData.Count; i++)
            {
                var info = world.LevelData.GetTile(i);

                _tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
            }

            for (int i = 0; i < world.Entities.Count; i++)
            {
                var ent = world.Entities[i];

                var obj = _prefabInstantiator.InstanceEntityByID(ent.Item1);
                obj.Transform.Position = ent.Item2;

                if (ent.Item1 == EntityID.ChestEmpty)
                {
                    var tile = _tilemap.GetTile(ent.Item2, 0);

                    if(tile != null)
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

        public void ChangeToLevel(string name)
        {
            DTime.TimeScale = 0;

            _screenUI.FadeIn(() =>
            {
                if (_player.IsPlayerDead)
                {
                    _player.Init();
                }

                if (_worldsData.TryGetValue(name, out var world))
                {
                    Load(world);
                }
                else
                {
                    Debug.Log("Can't load level!: No level with name: '" + name + "' exist");
                }

                _screenUI.FadeOut(() =>
                {
                    DTime.TimeScale = 1;
                });
            });
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

            _cursor.Entity.Transform.Position = DInput.GetMouseWorldPos();
            if (_camera.IsInside(DInput.GetMouseWorldPos(), Vector2.one * 0.01f))
            {
                Cursor.SetCursor(Texture2D.whiteTexture, Vector2.one, CursorMode.ForceSoftware);
                EditorGUIUtility.AddCursorRect(new Rect(0,0, Screen.width, Screen.height), MouseCursor.CustomCursor);
                Debug.Log("inside");
            }
            else
            {
                Debug.Log("outside");
            }

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
            return _currentWorld.LevelData.GetLevelTileData(vector.Round());
        }
    }
}