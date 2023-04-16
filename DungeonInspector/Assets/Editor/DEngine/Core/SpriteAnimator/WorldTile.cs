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
    public enum TileType
    {
        Static,
        Interactable,
        InteractableWhenTouch
    }

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

    [Serializable]
    public class DTile
    {
        //private string _textureName;
        //private int _zSorting = 0;
        //private bool _isWalkable;
        //private TileType _type;

        //private SpriteAnimation _idleAnimation; // use array with the names of the textures
        //private SpriteAnimation _interactableAnimation;

        private string[] _idleTexAnim;
        private string[] _interactableTexAnim;

        //public string Texture => _textureName;
        //public bool IsWalkable { get; set; }
        //public int ZSorting => _zSorting;
        //public TileType Type => _type;

        public int Index { get; set; }
        public string Texture { get; set; }
        public bool IsWalkable { get; set; }
        public int ZSorting { get; set; }
        public TileType Type { get; set; }
    }
}