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
        private E_SpriteAtlas _worldSpriteAtlas;
        private List<(DTile, Texture2D)> _tiles;
        public int Count => _tiles.Count;

        public TilesDatabase()
        {
            _tiles = new List<(DTile, Texture2D)>();

            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");

            for (int i = 0; i < _worldSpriteAtlas.TextureCount; i++)
            {
                var tex = _worldSpriteAtlas.GetTexture(i);

                var tile = new DTile()
                {
                    Index = i,
                    IsWalkable = false,
                    Type = TileType.Static,
                    Texture = tex.name,
                    ZSorting = 0,
                };

                _tiles.Add((tile, tex));
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
