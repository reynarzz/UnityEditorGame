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

        public TilesDatabase(string tilesGroupPath)
        {
            _tiles = new List<(DTile, Texture2D)>();

            if (!string.IsNullOrEmpty(tilesGroupPath))
            {
                _worldSpriteAtlas = Resources.Load<TilesGroup>(tilesGroupPath);

                for (int i = 0; i < _worldSpriteAtlas.TextureCount; i++)
                {
                    var tileData = _worldSpriteAtlas.GetTile(i);

                    var textureName = tileData.Texture ? tileData.Texture.name: tileData.Animation.GetSpriteNames()[0];

                    var tile = new DTile()
                    {
                        Index = i,
                        IsWalkable = tileData.Tile.IsWalkable,
                        Type = tileData.Tile.Type,
                        TextureName = textureName,
                        ZSorting = tileData.Tile.ZSorting,
                        TileBehavior = tileData.Tile.TileBehavior,
                        IdleTexAnim = tileData.Animation?.GetSpriteNames() ?? null
                    };

                    _tiles.Add((tile, tileData.Texture));
                }
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
