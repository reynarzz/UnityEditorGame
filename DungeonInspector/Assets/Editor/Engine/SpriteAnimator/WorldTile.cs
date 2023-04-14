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

    public class WorldTile
    {
        public Texture2D Texture { get; set; }
        public TileType Type { get; set; }
        public Vector2Int WorldPosition { get; set; }

        [SerializeField] private SpriteAnimation _idleAnimation;
        [SerializeField] private SpriteAnimation _interactableAnimation;

        public int Depth { get; set; }

        public void OnPlayerEnter()
        {

        }

        public void OnPlayerExit()
        {

        }
    }
}