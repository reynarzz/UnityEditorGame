using DungeonInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    [CustomEditor(typeof(WorldEditor))]
    public class WorldEditorEditor : Editor
    {
        private Vector2 _scroll;

        private E_SpriteAtlas _worldSpriteAtlas;
        private static Texture2D _selectedTex;

        public static Texture2D SelectedTex => _selectedTex;
        private void OnEnable()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");
            _selectedTex = _worldSpriteAtlas.GetTexture(0);

        }

        public override void OnInspectorGUI()
        {
            _scroll = GUILayout.BeginScrollView(_scroll);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < _worldSpriteAtlas.TextureCount; i++)
            {
                var tex = _worldSpriteAtlas.GetTexture(i);
                if (GUILayout.Button(new GUIContent(tex, tex.name), GUILayout.MinHeight(40)))
                {
                    _selectedTex = tex;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            if (GUILayout.Button("Save"))
            {

            }
        }

        private void EditTileType(WorldTile tile)
        {
            tile.Type = (TileType)EditorGUILayout.EnumPopup(tile.Type);

            //
        }
    }
}
