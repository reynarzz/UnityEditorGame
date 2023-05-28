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
        //private Player _player;

        protected override void OnStart()
        {
            _tilemap = GetComp<DTilemap>();
            //--var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");// gameMaster.TilesDatabase;
            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();// gameMaster.Camera;

            var playerObj = DGameEntity.FindGameEntity("Player");
            
            //if(playerObj != null)
            //{
            //    _player = playerObj.GetComp<Player>();
            //}
            


            DIEngineCoreServices.Get<DRendering>().AddCustomRenderControl(TestDraw_Remove);

        }

        // This should use the new render system.
        private void TestDraw_Remove()
        {
            foreach (var item in _tilemap.Tiles)
            {
                foreach (var s in item.Value)
                {
                    var tex = _tilesDatabase.GetTileTexture(s.Value.AssetIndex);

                    //if(_player != null)
                    //{
                    //    var playerRect = _camera.World2RectPos(_player.Transform.Position, _player.Transform.Scale);
                    //    //--_mat_DELETE.SetVector("_playerPos", new Vector4(playerRect.x, playerRect.y, playerRect.width, playerRect.height));

                    //}

                    Graphics.DrawTexture(_camera.World2RectPos(item.Key, Vector2.one), tex, _mat_DELETE);

                    //--_mat_DELETE.SetVector("_playerPos", default);
                }
            }
        }
    }
}
