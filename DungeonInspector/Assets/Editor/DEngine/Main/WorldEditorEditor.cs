using DungeonInspector;
using NUnit.Framework.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public struct SelectedTexInfo
    {
        public string TexIndex { get; set; }
        public Texture2D Tex { get; set; }
    }

    [CustomEditor(typeof(WorldEditor))]
    public class WorldEditorEditor : Editor
    {
        private Vector2 _scroll;

        private E_SpriteAtlas _worldSpriteAtlas;
        private static Texture2D _selectedTex;
        public static Action OnSave_Test;

        public static Texture2D SelectedTex => _selectedTex;
        private WorldTile _testTile_DELETE;


        private void OnEnable()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");
            _selectedTex =  _worldSpriteAtlas.GetTexture(0); 
            _testTile_DELETE = new WorldTile();
        }

        public override void OnInspectorGUI()
        {

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Pencil"))
            {

            }

            if (GUILayout.Button("Eraser"))
            {

            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(EditorStyles.helpBox);
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

            GUILayout.BeginVertical(EditorStyles.helpBox);

            EditTileType(_testTile_DELETE);
            GUILayout.EndVertical();

            GUILayout.EndVertical();

            if (GUILayout.Button("Save"))
            {
                OnSave_Test?.Invoke();
            }
        }

        private void EditTileType(WorldTile tile)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Tile Type");
            tile.SetValue("_type", (TileType)EditorGUILayout.EnumPopup(tile.Type, GUILayout.MaxWidth(190)));
            GUILayout.EndHorizontal();

            var rect = GUILayoutUtility.GetLastRect();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name");
            tile.SetValue("_textureName", _selectedTex.name); 
            EditorGUILayout.LabelField(_selectedTex.name, GUILayout.MaxWidth(190));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Is Walkable");
            tile.SetValue("_isWalkable", EditorGUILayout.Toggle(tile.IsWalkable, GUILayout.MaxWidth(190)));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Z Sorting");
            tile.SetValue("_zSorting", EditorGUILayout.IntSlider(tile.ZSorting, 0, 10, GUILayout.MaxWidth(190)));
            GUILayout.EndHorizontal();

            //SpriteAnimationOptions();

            //_idleAnimation;
            //_interactableAnimation;
            //_zSorting;
            //
        }

        //private void SpriteAnimationOptions()
        //{
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("Idle Animation");
        //    tile.SetValue("_idleAnimation", (SpriteAnimation)EditorGUILayout.ObjectField(tile.Texture, typeof(Texture2D), false, GUILayout.MaxWidth(90), GUILayout.MinHeight(90)));
        //    GUILayout.EndHorizontal();
        //}


    }
}
