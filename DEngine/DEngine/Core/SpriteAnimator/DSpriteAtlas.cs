using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public enum TileBehavior
    {
        None,
        Damage,
        IncreaseHealth,
        ChangeLevel
    }

    public enum TileType
    {
        Static,
        Interactable,
        InteractableWhenTouch
    }

    [CreateAssetMenu]
    public class DSpriteAtlas : ScriptableObject
    {
        [SerializeField] private Texture2D[] _textures;

        public int TextureCount => _textures.Length;

        public Texture2D GetTexture(int index)
        {
            return _textures[index];
        }
    }

    [Serializable]
    public class DTile
    {
        [SerializeField] private Texture2D _texture;

        [SerializeField] DSpriteAnimation _animation;
        public Texture2D Texture { get => _texture; set => _texture = value; }

        [NonSerialized] private string[] _interactableTexAnim;

        public string[] IdleTexAnim { get; set; }

        public int AssetIndex { get; set; }
        public int WorldIndex { get; set; }
        public string TextureName 
        {
            get 
            {
                if (_texture)
                {
                    return _texture.name;
                }
                else
                {
                    // get animation with texture name
                }
                return "Empty Texture";
            }
        }

        public object RuntimeData { get; set; }
        public DSpriteAnimation Animation { get; set; }

        public bool IsOccupied { get; set; }
        public bool IsWalkable;
        public int ZSorting;
        public TileType Type;
        public TileBehavior Behavior;
    }
}