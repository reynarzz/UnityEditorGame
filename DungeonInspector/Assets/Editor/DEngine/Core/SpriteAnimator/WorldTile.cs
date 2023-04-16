using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public enum TileType
    {
        Floor,
        Wall,
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
    public class WorldTile
    {
        private string _textureName;
        private int _zSorting = 0;
        private bool _isWalkable;
        private TileType _type;

        private SpriteAnimation _idleAnimation;
        private SpriteAnimation _interactableAnimation;

        public string Texture => _textureName;
        public bool IsWalkable => _isWalkable;
        public int ZSorting => _zSorting;
        public TileType Type => _type;
    }
}