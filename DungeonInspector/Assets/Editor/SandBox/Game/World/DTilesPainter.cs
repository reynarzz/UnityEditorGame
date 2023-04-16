using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace DungeonInspector
{
    public class DTilesPainter : DBehavior
    {
        private List<(TileDataInfo, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;
        private E_SpriteAtlas _worldSpriteAtlas;
        private DCamera _camera;
        private DGameMaster _gameMaster;
        private Material _mat_DELETE;

        public override void OnStart()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");

            _camera = FindGameEntity("Camera").GetComponent<DCamera>();

            _gameMaster = FindGameEntity("GameMaster").GetComponent<DGameMaster>();
            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");

            var tiles = _gameMaster.CurrentWorldData;

            _tiles = new List<(TileDataInfo, Texture2D)>();

            //for (int i = 0; i < length; i++)
            //{

            //}
            WorldEditorEditor.OnSave_Test = OnSave;
        }

        private void OnSave()
        {
            if (_tiles.Count > 0)
            {
                var tiles = new Dictionary<DVector2, List<TileDataInfo>>();

                for (int i = 0; i < _tiles.Count; i++)
                {
                    var worldPosition = _tiles[i].Item1;
                    var texName = _tiles[i].Item2.name;

                    var data = new TileDataInfo() { TileName = texName, WorldPosition = worldPosition.WorldPosition };

                    tiles.Add(data.WorldPosition, new List<TileDataInfo>() { data });
                }

                //for (int i = 0; i < _tiles.Count; i++)
                //{
                //    var worldPosition = _tiles[i].Item1;
                //    var texName = _tiles[i].Item2.name;

                //    tiles[i] = new TileDataInfo() { TileName = texName, WorldPosition = (DVector2)worldPosition };
                //}

                var worldData = new WorldData(tiles);
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

                File.WriteAllText(worldLevelPath, json);
                Debug.Log(json);
            }
        }

        public override void OnUpdate()
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
                Graphics.DrawTexture(_camera.World2RectPos(_tiles[i].Item1.WorldPosition, Vector2.one), _tiles[i].Item2, _mat_DELETE);

            }

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);
            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), WorldEditorEditor.SelectedTex, _mat_DELETE);
        }

        // Improve input system and all this.
        private void SetTile()
        {
            if (/*Event.current.type == EventType.MouseDown &&*/ Event.current.isMouse && Event.current.button == 0)
            {
                if (!_tiles.Exists(x => x.Item1.WorldPosition == _mouseTileGuidePosition))
                    _tiles.Add((new TileDataInfo() { IsWalkable = true, WorldPosition = _mouseTileGuidePosition }, WorldEditorEditor.SelectedTex));
            }
        }
    }
}
