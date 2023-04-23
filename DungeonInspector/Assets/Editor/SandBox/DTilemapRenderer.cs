using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace DungeonInspector
{
    public class DTilemapRenderer : DBehavior
    {
        private DTilemap _tilemap;
        private Material _mat_DELETE;
        private DCamera _camera;
        private TilesDatabase _tilesDatabase;
        private Player _player;

        protected override void OnStart()
        {
            _tilemap = GetComp<DTilemap>();
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            _tilesDatabase = gameMaster.TilesDatabase;
            _camera = gameMaster.Camera;

            _player = DGameEntity.FindGameEntity("Player").GetComp<Player>();

            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");

            DIEngineCoreServices.Get<DRenderingController>().AddCustomRenderControl(TestDraw_Remove);

        }

        // this should use the new render system.
        private void TestDraw_Remove()
        {
            foreach (var item in _tilemap.Tiles)
            {
                foreach (var s in item.Value)
                {
                    var tex = _tilesDatabase.GetTileTexture(s.Value.AssetIndex);

                    var playerRect = _camera.World2RectPos(_player.Transform.Position, _player.Transform.Scale);

                    _mat_DELETE.SetVector("_playerPos", new Vector4(playerRect.x, playerRect.y, playerRect.width, playerRect.height));
                    Graphics.DrawTexture(_camera.World2RectPos(item.Key, Vector2.one), tex, _mat_DELETE);

                    _mat_DELETE.SetVector("_playerPos", default);
                }
            }
        }
    }
}
