using DungeonInspector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonInspector
{
    public enum DTilePainterMode
    {
        Select,
        Brush,
        Eraser
    }

    public class DWorldEditor : DBehavior
    {
        private Vector2 _tilesScroll;
        private Vector2 _scroll;
        private (DTile, Texture2D) _selectedTile;

        private DTilemap _tilemap;

        private TilesDatabase _staticTiles;
        private TilesDatabase _animatedTiles;
        private DCamera _camera;

        public DTilePainterMode Mode { get; private set; } = DTilePainterMode.Eraser;

        private string[] _modes;

        private Vector2Int _mouseTileGuidePosition;
        private Material _mat_DELETE;
        private Texture2D _selectionFrame;
        private GUIStyle _style;

        private TileDatabaseType _type;

        private enum TileDatabaseType
        {
            Static,
            Animated,
            Interactable,
        }

        protected override void OnStart()
        {
            var gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();

            _tilemap = gameMaster.Tilemap;

            _staticTiles = gameMaster.TilesDatabase;
            _animatedTiles = gameMaster.AnimatedTiles;

            _camera = gameMaster.Camera;

            _selectedTile = _staticTiles.GetTileAndTex(0);

            _modes = new string[] { "Select", "Brush", "Eraser" };



            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");
            _selectionFrame = Resources.Load<Texture2D>("GameAssets/LevelEditor/SelectionFrame");
        }

        private void MousePointer()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            if (Mode != DTilePainterMode.Select)
            {
                _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));
            }

            var tex = default(Texture2D);

            var isInside = _camera.IsInside(newMousePos, Vector2.one * 0.01f);

            if (/*Event.current.type == EventType.MouseDown &&*/Event.current.isMouse && isInside)
            {
                if (Mode == DTilePainterMode.Brush)
                {
                    _tilemap.SetTile(_selectedTile.Item1, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
                else if (Mode == DTilePainterMode.Eraser)
                {
                    _tilemap.RemoveTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
                else if (Event.current.type == EventType.MouseDown)
                {
                    _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));
                }
            }

            tex = Mode == DTilePainterMode.Brush ? _selectedTile.Item2 : _selectionFrame;


            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);

            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), tex, _mat_DELETE);

            //Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), _selectionFrame, _mat_DELETE);

        }



        protected override void OnUpdate()
        {

            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label);
                _style.fontSize = 20;
            }

            MousePointer();


            Mode = (DTilePainterMode)GUILayout.Toolbar((int)Mode, _modes);

            TilesPicker();

            if (Mode == DTilePainterMode.Select)
                ShowWorldTileData();

            if (GUILayout.Button("Save"))
            {
                OnSave();
            }
        }


        private void TilesPicker()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Tiles");
            _type = (TileDatabaseType)EditorGUILayout.EnumPopup(_type);
            GUILayout.EndHorizontal();

            var tileDatabase = default(TilesDatabase);

            switch (_type)
            {
                case TileDatabaseType.Static:
                    tileDatabase = _staticTiles;
                    break;
                case TileDatabaseType.Interactable:
                    tileDatabase = _animatedTiles;
                    break;
                case TileDatabaseType.Animated:
                    break;
            }

            if (tileDatabase != null)
            {
                _scroll = GUILayout.BeginScrollView(_scroll);

                GUILayout.BeginHorizontal();


                for (int i = 0; i < tileDatabase.Count; i++)
                {
                    var tilePair = tileDatabase.GetTileAndTex(i);

                    var tex = tilePair.Item2;

                    var color = GUI.backgroundColor;

                    if (_selectedTile.Item1 == tilePair.Item1)
                    {
                        GUI.backgroundColor = Color.black * 0.4f;
                    }

                    //TODO: change the texture animation for animated tiles.
                    if (GUILayout.Button(new GUIContent(tex, tilePair.Item1.TextureName), GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        _selectedTile = tilePair;
                    }

                    GUI.backgroundColor = color;
                }

                GUILayout.EndHorizontal();

                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();

        }

        private void OnSave()
        {
            if (_tilemap.Tiles.Count > 0)
            {
                var tiles = new List<TileData>();
                var tileBehaviorData = new List<BaseTD>();

                foreach (var tile in _tilemap.Tiles)
                {
                    foreach (var item in tile.Value)
                    {
                        var position = tile.Key;

                        tiles.Add(new TileData() { Index = item.Value.Index, Position = position });
                        tileBehaviorData.Add(item.Value.RuntimeData);
                    }
                }

                var worldData = new LevelData(tiles.ToArray(), tileBehaviorData.ToArray());
                var json = JsonConvert.SerializeObject(worldData, Formatting.Indented);

                var worldLevelPath = Application.dataPath + "/Resources/World1.txt";

                File.WriteAllText(worldLevelPath, json);
                Debug.Log(json);
            }
        }

        private void ShowWorldTileData()
        {
            var tile = _tilemap.GetTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y, 0);
            if (tile != null)
            {
                GUILayout.BeginVertical();

                var texture = _staticTiles.GetTileTexture(tile.Index);

                GUILayout.BeginHorizontal();
                GUILayout.Label(texture, _style);
                GUILayout.Label(tile.TextureName);
                GUILayout.Label("Asset Index: " + tile.Index);
                GUILayout.EndHorizontal();

                RenderMembers(_value);

                GUILayout.EndVertical();

            }
        }
        private StringDataTD _value = new StringDataTD();

        private void RenderMembers(object type)
        {
            if (type != null)
            {

                var props = type.GetType().GetProperties();

                for (int i = 0; i < props.Length; i++)
                {
                    var property = props[i];
                    var name = property.Name;
                    var value = property.GetValue(type);
                    var propType = property.PropertyType;

                    if (propType.IsValueType)
                    {
                        if (propType.IsAssignableFrom(typeof(float)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name);
                            value = EditorGUILayout.FloatField((float)value);
                            GUILayout.EndHorizontal();
                        }
                        else if (propType.IsAssignableFrom(typeof(int)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name);
                            value = EditorGUILayout.IntField((int)value);
                            GUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        if (propType.IsAssignableFrom(typeof(string)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name);
                            value = EditorGUILayout.TextField((string)value);
                            GUILayout.EndHorizontal();
                        }
                    }

                    property.SetValue(type, value);
                }
            }
        }

        private void EditTileType(DTile tile)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

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


            GUILayout.EndVertical();
        }
    }
}
