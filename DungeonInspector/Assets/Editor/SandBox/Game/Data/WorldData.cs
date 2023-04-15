using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace DungeonInspector
{
    [Serializable]
    public class WorldData
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        private Dictionary<DVector2, List<TileDataInfo>> _data;

        // Newtonsoft json needs the default constructor
        private WorldData() { }
        public WorldData(Dictionary<DVector2, List<TileDataInfo>> data)
        {
            _data = data;
        }

        public TileDataInfo GetTile(int x, int y, int zSorting)
        {
            var layer = GetTileLayers(x, y);

            if (layer != null)
            {
                if (layer.Length > zSorting)
                {
                    return layer[zSorting];
                }
                else
                {
                    $"Cannot find tile in ZSorting layer: {zSorting}".LOGError();
                }
            }

            return default;
        }

        public TileDataInfo[] GetTileLayers(int x, int y)
        {
            if (_data.TryGetValue(new DVector2(x, y), out var tilesLayers))
            {
                return tilesLayers.ToArray();
            }

            return default;
        }


        public bool IsWalkable(int x, int y)
        {
            var layers = GetTileLayers(x, y);

            if (layers != null)
            {
                var walkable = true;

                for (int i = 0; i < layers.Length; i++)
                {
                    if (!layers[i].IsWalkable)
                    {
                        walkable = false;
                        break;
                    }
                }

                return walkable;
            }
            else
            {
                return false;
            }

            
        }
        // Data about all the tiles indexes.
    }

    [Serializable]
    public class LevelData
    {
        // what is this level about
    }

    [Serializable]
    public class PlayerData
    {
        // Data about the player
        public DVector2 Position { get; set; }
    }

    [Serializable]
    public class PlayerRuntimeData
    {
        // Player Data that will be erased once the player dies. (items collected)
    }

    [Serializable]
    public struct TileDataInfo
    {
        public bool IsWalkable { get; set; }
        public string TileName { get; set; }
        public DVector2 WorldPosition { get; set; }
    }
}
