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
