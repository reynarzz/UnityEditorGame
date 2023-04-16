using DungeonInspector;
using NUnit.Framework.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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
        private static (DTile, Texture2D) _selectedTile;
        public static Action OnSave_Test;

        public static (DTile, Texture2D) SelectTile => _selectedTile;

        private List<(DTile, Texture2D)> _tiles;

        private void OnEnable()
        {
            _worldSpriteAtlas = Resources.Load<E_SpriteAtlas>("World/World1");
            _tiles = new List<(DTile, Texture2D)>();

            for (int i = 0; i < _worldSpriteAtlas.TextureCount; i++)
            {
                var tex = _worldSpriteAtlas.GetTexture(i);
                var tile = new DTile()
                {
                    Index = i,
                    IsWalkable = false,
                    Type = TileType.Static,
                    Texture = tex.name,
                    ZSorting = 0,
                };

                _tiles.Add((tile, tex));
            }

            _selectedTile = _tiles[0];
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
            for (int i = 0; i < _tiles.Count; i++)
            {
                var tex = _tiles[i].Item2;
                var tile = _tiles[i].Item2;

                if (GUILayout.Button(new GUIContent(tex, tex.name), GUILayout.MinHeight(40)))
                {
                    _selectedTile = _tiles[i];
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            GUILayout.BeginVertical(EditorStyles.helpBox);

            EditTileType(_selectedTile.Item1);
            GUILayout.EndVertical();

            GUILayout.EndVertical();

            if (GUILayout.Button("Save"))
            {
                OnSave_Test?.Invoke();
            }
        }

        private void EditTileType(DTile tile)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Tile Type");
            tile.Type = (TileType)EditorGUILayout.EnumPopup(tile.Type, GUILayout.MaxWidth(190));
            GUILayout.EndHorizontal();

            var rect = GUILayoutUtility.GetLastRect();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name");
            EditorGUILayout.LabelField(tile.Texture, GUILayout.MaxWidth(190));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Is Walkable");
            tile.IsWalkable = EditorGUILayout.Toggle(tile.IsWalkable, GUILayout.MaxWidth(190));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Z Sorting");
            tile.ZSorting = EditorGUILayout.IntSlider(tile.ZSorting, 0, 10, GUILayout.MaxWidth(190));
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
