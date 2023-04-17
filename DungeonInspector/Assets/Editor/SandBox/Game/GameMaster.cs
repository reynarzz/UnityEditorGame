using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonInspector
{
    public class DGameMaster : DBehavior
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


        private TileBehaviorsContainer _tbContainer;


        private Dictionary<TileBehavior, List<Actor>> _tilesBehaviors;
        protected override void OnAwake()
        {
            _tilemap = FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _enemyDatabase = new EnemyDatabase();
            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Actor>>();
        }

        protected override void OnUpdate()
        {
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
                    behavior.OnUpdate(item.Value[i]);
                }
            }
        }

        public void OnActorEnterTile(Actor player, DTile tile)
        {
            var behavior = _tbContainer.GetBehavior(tile.TileBehavior);

            behavior.OnEnter(player);

            if (_tilesBehaviors.TryGetValue(tile.TileBehavior, out var playersList))
            {
                if (!playersList.Contains(player))
                {
                    playersList.Add(player);
                }
            }
            else
            {
                _tilesBehaviors.Add(tile.TileBehavior, new List<Actor>() { player });
            }
        }

        public void OnActorExitTile(Actor player, DTile tile)
        {
            var behavior = _tbContainer.GetBehavior(tile.TileBehavior);

            behavior.OnExit(player);

            if (_tilesBehaviors.TryGetValue(tile.TileBehavior, out var playersList))
            {
                playersList.Remove(player);

                if (playersList.Count == 0)
                {
                    _tilesBehaviors.Remove(tile.TileBehavior);
                }
            }
        }
    }
}