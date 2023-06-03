using Microsoft.Win32;
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

        public DTile GetNewTile(TileData data)
        {
            var tile = GetTile(data.TileAssetIndex);

            return new DTile()
            {
                AssetIndex = data.TileAssetIndex,
                Position = data.Position,
                WorldIndex = data.WorldIndex,
                Behavior = tile.Behavior,
                Type = tile.Type,
                ZSorting = tile.ZSorting,
                IsWalkable = tile.IsWalkable,
                Texture = tile.Texture,
                Animation = tile.Animation,
                RuntimeData = data.TileBehaviorData,
                AnimationAtlas = tile.AnimationAtlas,
                AnimationSpeed = tile.AnimationSpeed,
            };
        }

        public Texture2D GetTileTexture(int index)
        {
            return _tilesAsset.GetTile(index).Texture;
        }
    }
}