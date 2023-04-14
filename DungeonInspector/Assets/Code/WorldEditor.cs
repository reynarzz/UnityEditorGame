using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace InspGame
{
    public class WorldEditor : MonoBehaviour {  }

    [CustomEditor(typeof(WorldEditor))]
    public class WorldEditorEditor : Editor
    {
        private Vector2 _scroll;

        private void OnEnable()
        {
            //Resources.Load<E_SpriteAtra>
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
        }
    }
}