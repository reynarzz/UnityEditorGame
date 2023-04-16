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
    public class EnvironmentData
    {
        [JsonProperty] private TileInfo[] _tiles;

        public int Count => _tiles.Length;

        // Newtonsoft json needs the default constructor
        private EnvironmentData() { }
        public EnvironmentData(TileInfo[] tiles)
        {
            _tiles = tiles;
        }

        public TileInfo GetTile(int index)
        {
            return _tiles[index];
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
