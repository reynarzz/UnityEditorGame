using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
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
    public class TileData
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private DTile _tile;

        [SerializeField] DSpriteAnimation _animation;
        public Texture2D Texture => _texture;
        public DTile Tile => _tile;
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
