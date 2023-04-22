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
        private Dictionary<DVector2, Dictionary<int, DTile>> _tiles;
        public Dictionary<DVector2, Dictionary<int, DTile>> Tiles => _tiles;

        private DAABB _tilemapBounds;
        public int Count => _tiles.Count;

        protected override void OnAwake()
        {
            _tiles = new Dictionary<DVector2, Dictionary<int, DTile>>();
        }

        private int _index_REMOVE;
        public void SetNewTile(DTile tile, float x, float y)
        {
            var copy = new DTile() 
            {
                AssetIndex = tile.AssetIndex,

                WorldIndex = _index_REMOVE++, // TODO---------------------------------
                
                IsWalkable = tile.IsWalkable,
                Animation = tile.Animation,
                Behavior = tile.Behavior,
                Type = tile.Type,
                ZSorting = tile.ZSorting,
                Texture = tile.Texture,
                RuntimeData = tile.RuntimeData,
                IdleTexAnim = tile.IdleTexAnim,
            };

            

            SetTile(copy, x, y);
        }

        public void SetTile(DTile tile, float x, float y)
        {
            var pos = new DVector2((int)x, (int)y);

            if (_tiles.TryGetValue(pos, out var layers))
            {
                if (!layers.TryGetValue(tile.ZSorting, out var tileData))
                {
                    layers.Add(tile.ZSorting, tile);
                }
                else
                {
                    layers[tile.ZSorting] = tileData;
                }
            }
            else
            {
                var dict = new Dictionary<int, DTile>();

                dict.Add(tile.ZSorting, tile);

                _tiles.Add(pos, dict);
            }

            if (x < _tilemapBounds.Min.x)
            {
                _tilemapBounds.Min = new DVector2(x, _tilemapBounds.Min.y);
            }

            if (y < _tilemapBounds.Min.y)
            {
                _tilemapBounds.Min = new DVector2(_tilemapBounds.Min.x, y);
            }

            if (x > _tilemapBounds.Max.x)
            {
                _tilemapBounds.Max = new DVector2(x , _tilemapBounds.Max.y);
            }

            if (y > _tilemapBounds.Max.y)
            {
                _tilemapBounds.Max = new DVector2(_tilemapBounds.Max.x, y);
            }
        }


        public void RemoveTile(float x, float y)
        {
            var pos = new DVector2((int)x, (int)y);

            if (_tiles.TryGetValue(pos, out var layers))
            {
                _tiles.Remove(pos);
            }
        }

        public DTile GetTile(DVector2 position, int zSorting)
        {
            return GetTile(position.x, position.y, zSorting);
        }


        public DTile GetTile(float x, float y, int zSorting)
        {
            var layer = GetTileLayers((int)x, (int)y);

            if (layer != null)
            {
                if (layer.TryGetValue(zSorting, out var tile))
                {
                    return tile;
                }
                else
                {
                    Debug.Log($"Cannot find tile in ZSorting layer: {zSorting}");
                }
            }

            return default;
        }

        public Dictionary<int, DTile> GetTileLayers(int x, int y)
        {
            if (_tiles.TryGetValue(new DVector2(x, y), out var tilesLayers))
            {
                return tilesLayers;
            }

            return null;
        }

        public bool IsTileWalkable(int x, int y)
        {
            var layers = GetTileLayers(x, y);

            if (layers != null)
            {
                var walkable = true;

                for (int i = 0; i < layers.Values.Count; i++)
                {
                    if (!layers.Values.ElementAt(i).IsWalkable)
                    {
                        walkable = false;
                        break;
                    }
                }

                return walkable;
            }

            return false;
        }

        public DAABB GetTilemapBoundaries()
        {
            return _tilemapBounds;
        }

        public List<DTile> GetAllTiles()
        {
            var tiles = new List<DTile>();

            foreach (var tilesInLayer in _tiles.Values)
            {
                foreach (var tile in tilesInLayer.Values)
                {
                    tiles.Add(tile);
                }
            }

            return tiles;
        }

    }
}
