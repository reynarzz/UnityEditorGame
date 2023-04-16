using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PlasticGui.PlasticTableColumn;

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
    public struct TileInfo
    {
        public int Index { get; set; }
        /// <summary>In wolrd position</summary>
        public DVector2 Position { get; set; }
    }

    
}