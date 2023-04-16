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

        private TileBehaviorsContainer _tbContainer;


        private Dictionary<TileBehavior, List<Player>> _tilesBehaviors;
        public override void OnStart()
        {
            _tilemap = FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase();

            _tbContainer = new TileBehaviorsContainer();
            _tilesBehaviors = new Dictionary<TileBehavior, List<Player>>();
        }

        public override void OnUpdate()
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

        public void OnPlayerEnterTile(Player player, DTile tile)
        {
            //var tile = _tilemap.GetTile(_player.Transform.Position.x, _player.Transform.Position.y, 0);

            _tbContainer.GetBehavior(tile.TileBehavior).OnEnter(player);

            if (_tilesBehaviors.TryGetValue(tile.TileBehavior, out var playersList))
            {
                if (!playersList.Contains(player))
                {
                    playersList.Add(player);
                }
            }
            else
            {
                _tilesBehaviors.Add(tile.TileBehavior, new List<Player>() { player });
            }
        }

        public void OnPlayerExitTile(Player player, DTile tile)
        {
            _tbContainer.GetBehavior(tile.TileBehavior).OnExit(player);

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