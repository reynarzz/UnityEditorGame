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
        private List<(DTile, Texture2D, DVector2)> _tiles;

        private Vector2Int _mouseTileGuidePosition;
        private DCamera _camera;
        private DGameMaster _gameMaster;
        private Material _mat_DELETE;
        private DTilemap _tilemap;

        public override void OnStart()
        {

            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();
            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");
            _tilemap = _gameMaster.Tilemap;

            _tiles = new List<(DTile, Texture2D, DVector2)>();

            DWorldEditor.OnSave_Test = OnSave;
        }

        private void Load()
        {
            //_gameMaster.CurrentWorldData.GetTileLayers
            //_tiles
        }
        public override void OnUpdate()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

            AddTile();

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);
            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), DWorldEditor.SelectTile.Item2, _mat_DELETE);
        }

        // Improve input system and all this.
        private void AddTile()
        {
            if (/*Event.current.type == EventType.MouseDown &&*/ Event.current.isMouse && Event.current.button == 0)
            {
                _tilemap.SetTile(DWorldEditor.SelectTile.Item1, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
            }
        }

       
        private void OnSave()
        {
            if (_tiles.Count > 0)
            {
                var tiles = new TileInfo[_tiles.Count];

                for (int i = 0; i < _tiles.Count; i++)
                {
                    var worldPosition = _tiles[i].Item3;

                    tiles[i] = new TileInfo() { Index = _tiles[i].Item1.Index, Position = worldPosition };
                }

                var worldData = new EnvironmentData(tiles);
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

                File.WriteAllText(worldLevelPath, json);
                Debug.Log(json);
            }
        }


    }
}
