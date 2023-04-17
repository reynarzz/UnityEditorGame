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
    public class E_SpriteAtlas : ScriptableObject
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
        public Texture2D Texture => _texture;

        [NonSerialized] private string[] _interactableTexAnim;

        public string[] IdleTexAnim { get; set; }

        public int Index { get; set; }
        public string TextureName { get; set; }
        public bool IsWalkable;
        public int ZSorting;
        public TileType Type;
        public TileBehavior TileBehavior;
        public BaseTD RuntimeData { get; set; }
        public DSpriteAnimation Animation => _animation;
    }


    [CustomEditor(typeof(E_SpriteAtlas))]
    public class TlasEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            if (GUILayout.Button("Organize by name"))
            {
                //(target as E_SpriteAtlas).
                //_textures
            }

            base.OnInspectorGUI();


        }
    }
}
