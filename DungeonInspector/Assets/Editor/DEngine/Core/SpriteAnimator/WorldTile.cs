using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace DungeonInspector
{
    public enum InteractableTileType
    {
        AnimateOneTime,
        AnimateOnTouch,
        Disappear
    }

    public enum TileIdleAnimationType
    {
        None,
        Loop
    }

    [Serializable]
    public class TileData
    {
        public BaseTD TileBehaviorData { get; set; }

        // the Tile asset index
        public int TileAssetIndex { get; set; }
        /// <summary>In wolrd position</summary>
        public DVector2 Position { get; set; }
    }
}