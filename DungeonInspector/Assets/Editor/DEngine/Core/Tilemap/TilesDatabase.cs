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
        private TilesGroup _tilesAsset;
        public int Count => _tilesAsset.Count;

        public TilesDatabase(string tilesGroupPath)
        {
            if (!string.IsNullOrEmpty(tilesGroupPath))
            {
                _tilesAsset = Resources.Load<TilesGroup>(tilesGroupPath);
            }
        }

        public DTile GetTile(int index)
        {
            return _tilesAsset.GetTile(index);
        }

       
        public Texture2D GetTileTexture(int index)
        {
            return _tilesAsset.GetTile(index).Texture;
        }
    }
}