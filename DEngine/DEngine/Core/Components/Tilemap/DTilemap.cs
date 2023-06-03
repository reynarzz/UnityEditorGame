using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilemap : DBehavior
    {
        private Dictionary<DVec2, DTile> _tiles;
        public Dictionary<DVec2, DTile> Tiles => _tiles;

        private DAABB _tilemapBounds;
        public int Count => _tiles.Count;

        public DTilemap()
        {
            _tiles = new Dictionary<DVec2, DTile>();
        }

        //protected override void OnAwake()
        //{
        //    _tiles = new Dictionary<DVec2, Dictionary<int, DTile>>();
        //}

        private int _index_REMOVE;
        public void SetNewTile(DTile tile, float x, float y)
        {
            var copy = new DTile()
            {
                AssetIndex = tile.AssetIndex,
                Position = new DVec2((int)x, (int)y),

                WorldIndex = _index_REMOVE++, // TODO---------------------------------

                IsWalkable = tile.IsWalkable,
                Animation = tile.Animation,
                Behavior = tile.Behavior,
                Type = tile.Type,
                ZSorting = tile.ZSorting,
                Texture = tile.Texture,
                RuntimeData = tile.RuntimeData,
                AnimationAtlas = tile.AnimationAtlas,
                AnimationSpeed = tile.AnimationSpeed
            };



            SetTile(copy, x, y);
        }

        public void ChangeTileTexture(int x, int y, Texture2D texture)
        {
            var pos = new DVec2((int)x, (int)y);

            if (_tiles.TryGetValue(pos, out var tileOut))
            {
                tileOut.Texture = texture;
            }
        }

        public void SetTile(DTile tile, float x, float y)
        {
            var pos = new DVec2((int)x, (int)y);

            if (!_tiles.TryGetValue(pos, out var tileOut))
            {
                _tiles.Add(pos, tile);
            }
            else
            {
                _tiles[pos] = tile;
            }

            if (x < _tilemapBounds.Min.x)
            {
                _tilemapBounds.Min = new DVec2(x, _tilemapBounds.Min.y);
            }

            if (y < _tilemapBounds.Min.y)
            {
                _tilemapBounds.Min = new DVec2(_tilemapBounds.Min.x, y);
            }

            if (x > _tilemapBounds.Max.x)
            {
                _tilemapBounds.Max = new DVec2(x, _tilemapBounds.Max.y);
            }

            if (y > _tilemapBounds.Max.y)
            {
                _tilemapBounds.Max = new DVec2(_tilemapBounds.Max.x, y);
            }
        }

        public void RecalculateBounds()
        {
            _tilemapBounds = default;

            foreach (var pos in _tiles.Keys)
            {
                var x = pos.x;
                var y = pos.y;

                if (x < _tilemapBounds.Min.x)
                {
                    _tilemapBounds.Min = new DVec2(x, _tilemapBounds.Min.y);
                }

                if (y < _tilemapBounds.Min.y)
                {
                    _tilemapBounds.Min = new DVec2(_tilemapBounds.Min.x, y);
                }

                if (x > _tilemapBounds.Max.x)
                {
                    _tilemapBounds.Max = new DVec2(x, _tilemapBounds.Max.y);
                }

                if (y > _tilemapBounds.Max.y)
                {
                    _tilemapBounds.Max = new DVec2(_tilemapBounds.Max.x, y);
                }
            }

        }

        public void RemoveTile(float x, float y)
        {
            var pos = new DVec2((int)x, (int)y);

            if (_tiles.TryGetValue(pos, out var layers))
            {
                _tiles.Remove(pos);
            }
        }

        public DTile GetTile(float x, float y)
        {
            return GetTile(new DVec2((int)x, (int)y));
        }

        public DTile GetTile(DVec2 position)
        {
            if (_tiles.TryGetValue(position, out DTile tile))
            {
                return tile;
            }
            else
            {
                //Debug.Log($"Cannot find tile at position: {position}");

                return null;
            }
        }

        public bool IsTileWalkable(int x, int y)
        {
            var tile = GetTile(x, y);

            if(tile == null)
            {
                return false;
            }
            
            return tile.IsWalkable;
        }

        public DAABB GetTilemapBoundaries()
        {
            return _tilemapBounds;
        }

        public List<DTile> GetAllTiles()
        {
            return _tiles.Select(x => x.Value).ToList();
        }

        public void Clear()
        {
            _tiles.Clear();
        }
    }
}
