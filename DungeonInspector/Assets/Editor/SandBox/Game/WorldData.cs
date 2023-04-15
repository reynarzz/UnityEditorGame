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
        private readonly TileDataInfo[] _data;
      
        [JsonIgnore] public TileDataInfo[] TilesData => _data;

        // Newtonsoft json needs the default constructor
        private WorldData() { }
        public WorldData(TileDataInfo[] data)
        {
            _data = data;
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
    }

    [Serializable]
    public class PlayerRuntimeData
    {
        // Player Data that will be erased once the player dies. (items collected)
    }

    [Serializable]
    public struct TileDataInfo
    {
        public string TileName { get; set; }
        public DVector2 WorldPosition { get; set; }
    }
}
