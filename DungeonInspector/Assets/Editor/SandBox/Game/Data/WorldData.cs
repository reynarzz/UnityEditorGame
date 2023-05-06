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
    public class WorldData
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public LevelTilesData LevelData { get; set; }

        public List<(EntityID, DVec2)> Entities { get; set; }

        public WorldData()
        {
            Entities = new List<(EntityID, DVec2)>();
        }
    }

    [Serializable]
    public class LevelTilesData 
    {
        [JsonProperty] private TileData[] _tiles;
        [JsonIgnore] private Dictionary<DVec2, BaseTD> _levelTileData;
        public int Count => _tiles.Length;

        // Newtonsoft json needs the default constructor to deserialize.
        private LevelTilesData() { }

        public LevelTilesData(TileData[] tiles)
        {
            _tiles = tiles;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _levelTileData = new Dictionary<DVec2, BaseTD>();

            for (int i = 0; i < _tiles.Length; i++)
            {
                var data = _tiles.ElementAtOrDefault(i);

                //if (data.TileBehaviorData != null)
                //    Debug.Log(data.TileBehaviorData.GetType().Name);

                _levelTileData.Add(data.Position, (BaseTD)data.TileBehaviorData);
            }
        }

        public TileData GetTile(int saveIndex)
        {
            return _tiles[saveIndex];
        }

        public BaseTD GetLevelTileData(DVec2 position)
        {
            if (_levelTileData.TryGetValue(position, out var value))
            {
                return value;
            }

            return null;
        }

        public BaseTD GetLevelTileData(int saveIndex)
        {
            return (BaseTD)GetTile(saveIndex).TileBehaviorData;
        }
    }


    [Serializable]
    public class PlayerData
    {
        // Data about the player
        public DVec2 Position { get; set; }
    }

    [Serializable]
    public class PlayerRuntimeData
    {
        // Player Data that will be erased once the player dies. (items collected)
    }
}
