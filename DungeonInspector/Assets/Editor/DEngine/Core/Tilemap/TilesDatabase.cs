using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class TilesDatabase
    {
        private TilesGroup _worldSpriteAtlas;
        private List<(DTile, Texture2D)> _tiles;
        public int Count => _tiles.Count;

        public TilesDatabase()
        {
            _tiles = new List<(DTile, Texture2D)>();

            _worldSpriteAtlas = Resources.Load<TilesGroup>("World/World1Tiles");

            for (int i = 0; i < _worldSpriteAtlas.TextureCount; i++)
            {
                var tileData = _worldSpriteAtlas.GetTile(i);

                var tile = new DTile()
                {
                    Index = i,
                    IsWalkable = tileData.Tile.IsWalkable,
                    Type = tileData.Tile.Type,
                    TextureName = tileData.Texture.name,
                    ZSorting = 0,
                };

                _tiles.Add((tile, tileData.Texture));
            }

        }

        public (DTile, Texture2D) GetTileAndTex(int index)
        {
            return _tiles[index];
        }

        public DTile GetTile(int index)
        {
            return _tiles[index].Item1;
        }

        public Texture2D GetTileTexture(int index)
        {
            return _tiles[index].Item2;
        }
    }
}
