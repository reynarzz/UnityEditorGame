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
    public class LevelData
    {
        [JsonProperty] private TileData[] _tiles;

        [JsonProperty] private Dictionary<DVector2, BaseTD> _levelTileData;

        public int Count => _tiles.Length;

        // Newtonsoft json needs the default constructor
        private LevelData() { }
        public LevelData(TileData[] tiles)
        {
            _levelTileData = new Dictionary<DVector2, BaseTD>();

            _tiles = tiles;
        }

        public TileData GetTile(int saveIndex)
        {
            return _tiles[saveIndex];
        }

        public BaseTD GetLevelTileData(DVector2 position)
        {
            if (_levelTileData.TryGetValue(position, out var value))
            {
                return value;
            }

            return null;
        }
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
}
