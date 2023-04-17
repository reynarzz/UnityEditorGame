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

        protected override void OnAwake()
        {
            _tiles = new Dictionary<DVector2, Dictionary<int, DTile>>();
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

    }
}
