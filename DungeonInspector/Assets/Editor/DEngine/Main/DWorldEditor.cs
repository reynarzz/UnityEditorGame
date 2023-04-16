using DungeonInspector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public enum DTilePainterMode
    {
        Brush,
        Eraser
    }

    public class DWorldEditor : DBehavior
    {
        private Vector2 _scroll;
        private (DTile, Texture2D) _selectedTile;

        private DTilemap _tilemap;

        private TilesDatabase _tilesDatabase;
        private DCamera _camera;

        public DTilePainterMode Mode { get; private set; }

        private string[] _modes;

        private Vector2Int _mouseTileGuidePosition;
        private DGameMaster _gameMaster;
        private Material _mat_DELETE;

        public override void OnStart()
        {
            var gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();

            _tilemap = gameMaster.Tilemap;

            _tilesDatabase = gameMaster.TilesDatabase;
            _camera = gameMaster.Camera;

            _selectedTile = _tilesDatabase.GetTileAndTex(0);

            _modes = new string[] { "Brush", "Eraser" };


            
            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");

            Load();
        }

        private void MousePointer()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

            var tex = default(Texture2D);




            if (/*Event.current.type == EventType.MouseDown &&*/Event.current.isMouse)
            {
                if(Event.current.button == 0)
                {
                    Mode = DTilePainterMode.Brush;
                }
                else if(Event.current.button == 1)
                {
                    Mode = DTilePainterMode.Eraser;
                }
                
                if (Mode == DTilePainterMode.Brush)
                {

                    _tilemap.SetTile(_selectedTile.Item1, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
                else
                {
                    _tilemap.RemoveTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
            }

            tex = Mode == DTilePainterMode.Brush ? _selectedTile.Item2 : Texture2D.whiteTexture;


            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);
            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), tex, _mat_DELETE);
        }
        private void Load()
        {
            var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

            var json = File.ReadAllText(worldLevelPath);

            var data = JsonConvert.DeserializeObject<EnvironmentData>(json);

            for (int i = 0; i < data.Count; i++)
            {
                var info = data.GetTile(i);

                _tilemap.SetTile(_tilesDatabase.GetTile(info.Index), info.Position.x, info.Position.y);
            }
        }



        public override void OnUpdate()
        {
            MousePointer();
            //TODO: offset is wrong Fix this.
            GUILayout.Space(_camera.ScreenSize.y);

            //GUILayout.Space(250);
            //GUILayout.BeginArea(new Rect(_camera.ViewportRect.x, _camera.ViewportRect.y + _camera.ViewportRect.height, EditorGUIUtility.currentViewWidth, 200));

            Mode = (DTilePainterMode)GUILayout.Toolbar((int)Mode, _modes);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            _scroll = GUILayout.BeginScrollView(_scroll);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < _tilesDatabase.Count; i++)
            {
                var tilePair = _tilesDatabase.GetTileAndTex(i);

                var tex = tilePair.Item2;

                var color = GUI.backgroundColor;

                if (_selectedTile.Item1 == tilePair.Item1)
                {
                    GUI.backgroundColor = Color.black * 0.4f;
                }

                if (GUILayout.Button(new GUIContent(tex, tex.name), GUILayout.Width(40), GUILayout.Height(40)))
                {
                    _selectedTile = tilePair;
                }

                GUI.backgroundColor = color;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            GUILayout.BeginVertical(EditorStyles.helpBox);

            EditTileType(_selectedTile.Item1);
            GUILayout.EndVertical();

            GUILayout.EndVertical();

            if (GUILayout.Button("Save"))
            {
                OnSave();
            }

            //GUILayout.EndArea();
        }


        // Improve input system and all this.
        private void AddTile()
        {
        }

        private void OnSave()
        {
            if (_tilemap.Tiles.Count > 0)
            {
                var tiles = new List<TileInfo>();

                foreach (var tile in _tilemap.Tiles)
                {
                    foreach (var item in tile.Value)
                    {
                        var position = tile.Key;

                        tiles.Add(new TileInfo() { Index = item.Value.Index, Position = position });
                    }
                }

                var worldData = new EnvironmentData(tiles.ToArray());
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

                File.WriteAllText(worldLevelPath, json);
                Debug.Log(json);
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
            EditorGUILayout.LabelField(tile.TextureName, GUILayout.MaxWidth(190));
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
