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
        [SerializeField] private TileType _type;
        [SerializeField] private string _textureName;

        private SpriteAnimation _idleAnimation;
        private SpriteAnimation _interactableAnimation;
        [SerializeField] private int _zSorting = 0;

        public int ZSorting => _zSorting;
        public Vector2Int WorldPosition { get; set; }
        public string Texture => _textureName;
        public TileType Type => _type;
    }
}