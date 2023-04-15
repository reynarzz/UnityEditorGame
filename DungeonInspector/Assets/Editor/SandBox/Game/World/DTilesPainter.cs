using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilesPainter : DBehavior
    {
        private List<(Vector2, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;
        private DCamera _camera;

        public override void Start()
        {
            _camera = FindGameEntity("Camera").GetComponent<DCamera>();
        }

        public override void Update()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            if (_tiles == null)
                _tiles = new List<(Vector2, Texture2D)>();



            newMousePos.x = Mathf.RoundToInt(newMousePos.x);
            newMousePos.y = Mathf.RoundToInt(newMousePos.y);

            _mouseTileGuidePosition = new Vector2Int((int)newMousePos.x, (int)newMousePos.y);


            SetTile();

            //for (int i = 0; i < _tiles.Count; i++)
            //{
            //    DrawSprite(_tiles[i].Item1, new Vector2(1, 1), _camera_Test, _tiles[i].Item2);
            //}

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);
        }

        private void SetTile()
        {
            if (Event.current.isMouse && Event.current.button == 0)
            {
                _tiles.Add((_mouseTileGuidePosition, WorldEditorEditor.SelectedTex));
            }

        }

    }
}
