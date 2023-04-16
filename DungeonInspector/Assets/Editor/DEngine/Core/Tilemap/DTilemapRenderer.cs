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

        public override void OnStart()
        {
            _tilemap = GetComp<DTilemap>();
            var gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();
            _tilesDatabase = gameMaster.TilesDatabase;
            _camera = gameMaster.Camera;

            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");

        }

        public override void OnUpdate()
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
                    var tex = GetTexFromTile(s.Value);

                    Graphics.DrawTexture(_camera.World2RectPos(item.Key, Vector2.one), tex, _mat_DELETE);
                }
            }
        }

        private Texture2D GetTexFromTile(DTile tile)
        {
            return _tilesDatabase.GetTileTexture(tile.Index);
        }


    }
}
