using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilemapRenderer : DBehavior
    {
        private DTilemap _tilemap;
        private Material _mat_DELETE;
        private DCamera _camera;
        private TilesDatabase _tilesDatabase;

        protected override void OnStart()
        {
            _tilemap = GetComp<DTilemap>();
            var gameMaster = GameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();
            _tilesDatabase = gameMaster.TilesDatabase;
            _camera = gameMaster.Camera;

            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");

        }

        protected override void OnUpdate()
        {
            TestDraw_Remove();
        }

        // this should use the new render system.
        private void TestDraw_Remove()
        {
            foreach (var item in _tilemap.Tiles)
            {
                foreach (var s in item.Value)
                {
                    var tex = _tilesDatabase.GetTileTexture(s.Value.AssetIndex);

                    Graphics.DrawTexture(_camera.World2RectPos(item.Key, Vector2.one), tex, _mat_DELETE);
                }
            }
        }
    }
}
