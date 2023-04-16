using DungeonInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DWorldEditor : DBehavior
    {
        private Vector2 _scroll;
        private static (DTile, Texture2D) _selectedTile;

        public static Action OnSave_Test;

        public static (DTile, Texture2D) SelectTile => _selectedTile;

        private TilesDatabase _tilesDatabase;
        private DCamera _camera;

        public override void OnStart()
        {
            var gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();
            _tilesDatabase = gameMaster.TilesDatabase;
            _camera = gameMaster.Camera;

            _selectedTile = _tilesDatabase.GetTile(0);
        }

        public override void OnUpdate()
        {
            GUILayout.Space(200);
            GUILayout.BeginArea(new Rect(_camera.ViewportRect.x, _camera.ViewportRect.y + _camera.ViewportRect.height, EditorGUIUtility.currentViewWidth, 200));
           
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

            for (int i = 0; i < _tilesDatabase.Count; i++)
            {
                var tilePair = _tilesDatabase.GetTile(i);

                var tex = tilePair.Item2;

                if (GUILayout.Button(new GUIContent(tex, tex.name), GUILayout.MinHeight(40)))
                {
                    _selectedTile = tilePair;
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

            GUILayout.EndArea();
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
