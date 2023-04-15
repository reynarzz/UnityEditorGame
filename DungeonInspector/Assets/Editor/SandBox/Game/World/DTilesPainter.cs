using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace DungeonInspector
{
    public class DTilesPainter : DBehavior
    {
        private List<(Vector2, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;
        private E_SpriteAtlas _worldSpriteAtlas;
        private DCamera _camera;

        public override void OnStart()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");

            _camera = FindGameEntity("Camera").GetComponent<DCamera>();

            _tiles = new List<(Vector2, Texture2D)>();
            WorldEditorEditor.OnSave_Test = OnSave;
        }

        private void OnSave()
        {
            if(_tiles.Count > 0)
            {
                var tiles = new TileDataInfo[_tiles.Count];

                for (int i = 0; i < _tiles.Count; i++)
                {
                    var worldPosition = _tiles[i].Item1;
                    var texName = _tiles[i].Item2.name;

                    tiles[i] = new TileDataInfo() { TileName = texName, WorldPosition = (DVector2)worldPosition };
                }

                var worldData = new WorldData(tiles);
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                Debug.Log(json);
            }
        }

        public override void UpdateFrame()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

            SetTile();

            TestDraw_Remove();
        }

        // this should use the new render system.
        private void TestDraw_Remove()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                Graphics.DrawTexture(_camera.World2RectPos(_tiles[i].Item1, Vector2.one), _tiles[i].Item2);

            }

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);
            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), WorldEditorEditor.SelectedTex);
        }

        // Improve input system and all this.
        private void SetTile()
        {
            if (/*Event.current.type == EventType.MouseDown &&*/ Event.current.isMouse && Event.current.button == 0)
            {
                if(!_tiles.Exists(x => x.Item1 == _mouseTileGuidePosition))
                _tiles.Add((_mouseTileGuidePosition, WorldEditorEditor.SelectedTex));
            }

        }

    }
}
