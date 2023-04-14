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

        private void OnEnable()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("WorldAtlas/World1");
        }

        public override void OnInspectorGUI()
        {
            _scroll = GUILayout.BeginScrollView(_scroll);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < 10; i++)
            {
                if (GUILayout.Button(Texture2D.whiteTexture, GUILayout.MinHeight(40)))
                {

                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            if (GUILayout.Button("Save"))
            {

            }
        }
    }
}
