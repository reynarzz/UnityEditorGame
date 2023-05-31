using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class GameMaster : DBehavior
    {
        private DCamera _camera;
        public DTilemap Tilemap => _currentWorldController.Tilemap;
        public DCamera Camera => _camera;

        private TilesDatabase _tilesDatabase;
        private TilesDatabase _animatedTiles;


        private PrefabInstantiator _prefabInstantiator;
        private PlayerHealthUI _playerHealthUI;

        public PrefabInstantiator PrefabInstantiator => _prefabInstantiator;

        private TileBehaviorsContainer _tbContainer;

        private Dictionary<TileBehavior, List<Actor>> _tilesBehaviors;
        public NavWorld NavWorld => _currentWorldController.NavWorld;
        private Player _player;
        private ScreenUI _screenUI;
        //private WorldData _currentWorld;

        private GameInput _input;
        public GameInput Input => _input;
        private Texture2D _cursorTex;
        private DAtlasRendererComponent _cursor;

        private WorldControllerBase _currentWorldController;
        private WorldsManager _worldsContainer;
        private World _currentWorldType;

        protected override void OnAwake()
        {
            DTime.TimeScale = 1;
            DCamera.PixelSize = 32;


            // Main Player
            _player = new DGameEntity("Player", typeof(DSpriteRendererComponent), typeof(DAnimatorComponent), typeof(Player)).GetComp<Player>();

            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Actor>>();
            _prefabInstantiator = new PrefabInstantiator();
            _worldsContainer = new WorldsManager(_prefabInstantiator, _tilesDatabase);

            //_navWorld = new NavWorld(_tilemap);
            _screenUI = new DGameEntity("ScreenUI", typeof(DRendererUIComponent), typeof(ScreenUI)).GetComp<ScreenUI>();


            _playerHealthUI = new DGameEntity("PlayerHealthHUD").AddComp<PlayerHealthUI>();
            _cursorTex = Resources.Load<Texture2D>("GameAssets/GUI/GunSights");

            _cursor = new DGameEntity("Cursor").AddComp<DAtlasRendererComponent>();

            _cursor.AtlasInfo.Texture = _cursorTex;
            _cursor.AtlasInfo.BlockSIze = 5;
            _cursor.SpriteCoord = new DVec2(2, 1);
            _cursor.ZSorting = 30;
            _cursor.Transform.Scale = new DVec2(0.43f, 0.42f);

            _currentWorldType = World.Prologe;
            _currentWorldController = _worldsContainer.Load(World.Prologe);
        }



        private void OnPlayerDead()
        {
            ChangeToLevel(_currentWorldType, _player.Transform.Position);
        }

        protected override void OnStart()
        {
            //DAudio.PlayAudio("Audio/ForgottenPlains/Music/Plain_Sight_(Regular).wav");

            _player.GetComp<ActorHealth>().OnHealthDepleted += OnPlayerDead;
        }



        public void ChangeToLevel(World name, Vector2 spawnPosition = default)
        {
            DTime.TimeScale = 0;

            _screenUI.FadeIn(() =>
            {
                if (_player.IsPlayerDead)
                {
                    _player.Init();
                }

                _currentWorldController = _worldsContainer.Load(name);

                _player.SetPosition(spawnPosition);
                _camera.GetComp<DCameraFollow>().SetInstantPosition();

                _screenUI.FadeOut(() =>
                {
                    DTime.TimeScale = 1;

                });
            });
        }

        protected override void OnUpdate()
        {
            //-Utils.DrawBounds(_tilemap.GetTilemapBoundaries(), Color.white, 0.5f);

            _worldsContainer.Update();

            UpdateTilesBehavior();

            EditorGUILayout.Space(18);

        }

        protected override void OnLateUpdate()
        {
            _worldsContainer.LateUpdate();

            _cursor.Entity.Transform.Position = DInput.GetMouseWorldPos();

            if (_camera.IsInside(DInput.GetMouseWorldPos(), Vector2.one * 0.01f))
            {
                Cursor.SetCursor(Texture2D.whiteTexture, Vector2.one, CursorMode.ForceSoftware);
                EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), MouseCursor.CustomCursor);
                //Debug.Log("inside");
            }
            else
            {
                //Debug.Log("outside");
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
            // Todo: use tile manager here! not the last one!!
            return _currentWorldController.TilemapsData.Last().GetLevelTileData(vector.Round());
        }
    }
}