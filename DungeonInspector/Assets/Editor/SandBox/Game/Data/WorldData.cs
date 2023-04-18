using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DungeonInspector
{


    [Serializable]
    public class LevelData 
    {
        [JsonProperty] private TileData[] _tiles;
        [JsonIgnore] private Dictionary<DVector2, BaseTD> _levelTileData;

        public int Count => _tiles.Length;

        // Newtonsoft json needs the default constructor to deserialize.
        private LevelData() { }

        public LevelData(TileData[] tiles)
        {
            _tiles = tiles;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _levelTileData = new Dictionary<DVector2, BaseTD>();

            for (int i = 0; i < _tiles.Length; i++)
            {
                var data = _tiles.ElementAtOrDefault(i);

                _levelTileData.Add(data.Position, data.TileBehaviorData);
            }
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

        public BaseTD GetLevelTileData(int saveIndex)
        {
            return GetTile(saveIndex).TileBehaviorData;
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
