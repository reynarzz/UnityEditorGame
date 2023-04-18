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
        private DTile _selectedTile;

        private DTilemap _tilemap;

        private TilesDatabase _staticTiles;
        private TilesDatabase _animatedTiles;
        private DCamera _camera;

        public DTilePainterMode Mode { get; private set; } = DTilePainterMode.Select;

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
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            _tilemap = gameMaster.Tilemap;

            _staticTiles = gameMaster.TilesDatabase;
            _animatedTiles = gameMaster.AnimatedTiles;

            _camera = gameMaster.Camera;

            _selectedTile = _staticTiles.GetTile(0);

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
                    _tilemap.SetNewTile(_selectedTile, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
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
            
            tex = Mode == DTilePainterMode.Brush ? _selectedTile.Texture : _selectionFrame;


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
                    var tile = tileDatabase.GetTile(i);

                    var color = GUI.backgroundColor;

                    if (_selectedTile == tile)
                    {
                        GUI.backgroundColor = Color.black * 0.4f;
                    }

                    //TODO: change the texture animation for animated tiles.
                    if (GUILayout.Button(new GUIContent(tile.Texture, tile.Texture.name), GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        _selectedTile = tile;
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

                foreach (var tile in _tilemap.Tiles)
                {
                    foreach (var item in tile.Value)
                    {
                        var position = tile.Key;

                        tiles.Add(new TileData()
                        {
                            TileAssetIndex = item.Value.AssetIndex,
                            Position = position,
                            TileBehaviorData = item.Value.RuntimeData,
                            WorldIndex = tiles.Count
                        });
                    }
                }

                var worldData = new LevelData(tiles.ToArray());
                var json = JsonConvert.SerializeObject(worldData, typeof(BaseTD), Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

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

                var texture = _staticTiles.GetTileTexture(tile.AssetIndex);

                GUILayout.BeginHorizontal();
                GUILayout.Label(texture, _style);
                GUILayout.Label(tile.TextureName);
                GUILayout.Label("World Index: " + tile.WorldIndex);
                GUILayout.EndHorizontal();

                RenderMembers(GetDataSafe(tile));

                GUILayout.EndVertical();

            }
        }

        private BaseTD GetDataSafe(DTile tile)
        {

            if (tile.RuntimeData == null)
            {
                switch (tile.Behavior)
                {
                    case TileBehavior.Damage:
                        tile.RuntimeData = new IntDataTD();
                        break;
                    case TileBehavior.IncreaseHealth:
                        break;
                    case TileBehavior.ChangeLevel:
                        tile.RuntimeData = new ChangeLevelTD();
                        break;
                }
            }

            return tile.RuntimeData;
        }

        private void RenderMembers(BaseTD type)
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
