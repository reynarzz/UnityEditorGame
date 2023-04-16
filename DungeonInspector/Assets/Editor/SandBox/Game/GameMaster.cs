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

        private Player _player;

        private TileBehaviorsContainer _tbContainer;

        public override void OnStart()
        {
            _tilemap = FindGameEntity("TileMaster").GetComp<DTilemap>();
            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _tilesDatabase = new TilesDatabase();
            _player = FindGameEntity("Player").GetComp<Player>();

            _tbContainer = new TileBehaviorsContainer();
        }

        public override void OnUpdate()
        {

        }

        private void OnPlayerReachTile()
        {
            var tile = _tilemap.GetTile(_player.Transform.Position.x, _player.Transform.Position.y, 0);

            var behavior = _tbContainer.GetBehavior(tile.TileBehavior);

            // detect when player enters tile
            // detect when player exits tile
            // and how to update the tile properly
        }
    }
}