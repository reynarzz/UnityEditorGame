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
        private Vector2Int _mouseTileGuidePosition;
        private DCamera _camera;
        private DGameMaster _gameMaster;
        private Material _mat_DELETE;
        private DTilemap _tilemap;
        private TilesDatabase _tilesDatabase;
        public override void OnStart()
        {

            _camera = FindGameEntity("Camera").GetComp<DCamera>();

            _gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();
            _tilesDatabase = _gameMaster.TilesDatabase;

            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");
            _tilemap = _gameMaster.Tilemap;

            DWorldEditor.OnSave_Test = OnSave;

            Load();
        }

        private void Load()
        {
            var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

            var json = File.ReadAllText(worldLevelPath);

            var data = JsonConvert.DeserializeObject<EnvironmentData>(json);

            for (int i = 0; i < data.Count; i++)
            {
                var info = data.GetTile(i);

                _tilemap.SetTile(_tilesDatabase.GetTile(info.Index), info.Position.x, info.Position.y);
            }
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
            if (_tilemap.Tiles.Count > 0)
            {
                var tiles = new List<TileInfo>();

                foreach (var tile in _tilemap.Tiles)
                {
                    foreach (var item in tile.Value)
                    {
                        var position = tile.Key;

                        tiles.Add(new TileInfo() { Index = item.Value.Index, Position = position });
                    }
                }
               
                var worldData = new EnvironmentData(tiles.ToArray());
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

                File.WriteAllText(worldLevelPath, json);
                Debug.Log(json);
            }
        }


    }
}
