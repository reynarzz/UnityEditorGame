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
        private Vector2 _scroll;
        private DTile _selectedTile;

        private DTilemap _tilemap;

        private TilesDatabase _tilesDatabase;
        private TilesDatabase _animatedTiles;
        private DCamera _camera;

        public DTilePainterMode Mode { get; private set; } = DTilePainterMode.Select;

        private string[] _modes;

        private static Vector2Int _mouseTileGuidePosition;
        private Material _mat_DELETE;
        private Texture2D _selectionFrame;
        private GUIStyle _style;

        private TileDatabaseType _type;

        private string[] _menu = { "Painter", "Manager" };
        private const float _mouseDeltaSens = 0.01557f;
        private List<WorldData> _worlds;

        private string _worldNameCreate;
        private static int _selectedWorldIndex;
        private static DVec2 _cameraPos;

        private EntityInfo _currentPickedEntity;
        private EditModePrefabInstantiator _entityList;
        private GUIContent[] _entityPickerGuiContent;

        private enum TileDatabaseType
        {
            Static,
            Animated,
            Interactable,
        }

        protected override void OnStart()
        {
            var tilemap = DGameEntity.FindGameEntity("TileMaster").GetComp<DTilemap>();

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _entityList = new EditModePrefabInstantiator();

            _tilemap = tilemap;

            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _selectedTile = _tilesDatabase.GetTile(0);

            _modes = new string[] { "Select", "Brush", "Eraser" };

            var data = Resources.Load<TextAsset>("Data/WorldData");

            AssetDatabase.Refresh();
            if (data != null)
            {

                _worlds = JsonConvert.DeserializeObject<List<WorldData>>(data.text,
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                if (_worlds.Count > _selectedWorldIndex)
                {
                    Load(_worlds[_selectedWorldIndex].LevelData);
                }
            }
            else
            {
                _worlds = new List<WorldData>();
            }

            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");
            _selectionFrame = Resources.Load<Texture2D>("GameAssets/LevelEditor/SelectionFrame");

            _entityPickerGuiContent = new GUIContent[_entityList.Count];

            for (int i = 0; i < _entityList.Count; i++)
            {
                var tex = _entityList.GetEntityInfo(i).Tex;

                _entityPickerGuiContent[i] = new GUIContent(tex, tex.name);
            }
        }

        private void MousePointer()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            if (Mode != DTilePainterMode.Select)
            {
                _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

                if (DInput.IsMouseDown(0))
                {
                    Mode = DTilePainterMode.Brush;
                }
                else if (DInput.IsMouseDown(1))
                {
                    Mode = DTilePainterMode.Eraser;
                }
            }

            var tex = default(Texture2D);

            var isInside = _camera.IsInside(newMousePos, Vector2.one * 0.01f);

            if (isInside)
            {
                if (DInput.IsMouse(0) && Mode == DTilePainterMode.Brush)
                {
                    _tilemap.SetNewTile(_selectedTile, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
                else if ((DInput.IsMouse(0) || DInput.IsMouse(1)) && Mode == DTilePainterMode.Eraser)
                {
                    _tilemap.RemoveTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                    _tilemap.RecalculateBounds();
                }
                else if (DInput.IsMouse(0))
                {
                    _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));
                }
            }

            if (DInput.IsMouse(2))
            {
                var dt = DInput.MouseDelta;
                dt.x = -dt.x;

                _cameraPos += dt * _mouseDeltaSens;
            }

            _camera.Transform.Position = _cameraPos;
            tex = Mode == DTilePainterMode.Brush ? _selectedTile.Texture : _selectionFrame;

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);

            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), tex, _mat_DELETE);

            if(_currentPickedEntity != null)
            {
                Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), _currentPickedEntity.Tex, _mat_DELETE);
            }
            //Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), _selectionFrame, _mat_DELETE);

        }


        private int _menuSelected;

        protected override void OnUpdate()
        {
            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label);
                _style.fontSize = 20;
            }
            GUILayout.Space(30);

            _menuSelected = GUILayout.Toolbar(_menuSelected, _menu);
            MousePointer();

            if (_menuSelected == 0)
            {
                GUILayout.Label(_mouseTileGuidePosition.ToString());

                Mode = (DTilePainterMode)GUILayout.Toolbar((int)Mode, _modes);

                TilesPicker();
                EntityPicker();

                if (Mode == DTilePainterMode.Select)
                {
                    ShowWorldTileData();
                }

                GUILayout.BeginHorizontal();


                if (GUILayout.Button("Save"))
                {
                    OnSave();
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                LevelManager();
            }


            Utils.DrawBounds(_tilemap.GetTilemapBoundaries(), Color.white, 0.5f);
        }

        private DVec2 _pickerScroll;
        private int _selectedEntity = -1;

        private void EntityPicker()
        {
            GUILayout.BeginArea(new Rect(EditorGUIUtility.currentViewWidth - 70, 50, 60, 300), EditorStyles.helpBox);
            _pickerScroll = GUILayout.BeginScrollView(_pickerScroll, GUIStyle.none, GUIStyle.none);

            for (int i = 0; i < _entityPickerGuiContent.Length; i++)
            {
                var info = _entityPickerGuiContent[i];

                var c = GUI.backgroundColor;

                if(_selectedEntity == i)
                {
                    GUI.backgroundColor = new Color(0.4f, 0.8f, 1, 1);
                }

                if (GUILayout.Button(info, GUILayout.MaxWidth(45), GUILayout.MinHeight(30)))
                {
                    _selectedEntity = i;
                    _currentPickedEntity = _entityList.GetEntityInfo(_selectedEntity);
                }

                GUI.backgroundColor = c;
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }



        private Vector2 _levelManagerScroll;

        private void LevelManager()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("New", GUILayout.MaxWidth(30));
            _worldNameCreate = GUILayout.TextField(_worldNameCreate);

            if (GUILayout.Button("+", GUILayout.MaxWidth(25)))
            {
                if (!string.IsNullOrEmpty(_worldNameCreate))
                {
                    _worlds.Add(new WorldData() { Name = _worldNameCreate });

                    _worldNameCreate = null;

                    _selectedWorldIndex = _worlds.Count - 1;
                    _tilemap.Clear();
                    OnSave();
                }
                else
                {
                    Debug.Log("Set a name first!");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            _levelManagerScroll = GUILayout.BeginScrollView(_levelManagerScroll, EditorStyles.helpBox);

            for (int i = 0; i < _worlds.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(_worlds[i].Name))
                {
                    _selectedWorldIndex = i;

                    Load(_worlds[i].LevelData);
                }

                if (GUILayout.Button("X", GUILayout.MaxWidth(25)))
                {
                    _worlds.RemoveAt(i);
                    _selectedWorldIndex = 0;
                    _tilemap.Clear();
                    SaveChange();
                    GUILayout.EndHorizontal();
                    break;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void Load(LevelTilesData levelData)
        {
            if (levelData != null)
            {
                for (int i = 0; i < levelData.Count; i++)
                {
                    var info = levelData.GetTile(i);

                    _tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
                }
            }
            else
            {
                _tilemap.Clear();
            }
        }

        private void TilesPicker()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Tiles");
            //_type = (TileDatabaseType)EditorGUILayout.EnumPopup(_type);
            //GUILayout.EndHorizontal();

            //var tileDatabase = default(TilesDatabase);

            //switch (_type)
            //{
            //    case TileDatabaseType.Static:
            //        tileDatabase = _tilesDatabase;
            //        break;
            //    case TileDatabaseType.Interactable:
            //        tileDatabase = _animatedTiles;
            //        break;
            //    case TileDatabaseType.Animated:
            //        break;
            //}

            var tileDatabase = _tilesDatabase;

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
                        _currentPickedEntity = null;
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
            //if (_tilemap.Tiles.Count > 0)
            {
                var tiles = new List<TileData>();

                foreach (var tile in _tilemap.Tiles)
                {
                    foreach (var item in tile.Value)
                    {
                        var position = tile.Key;
                        //Debug.Log(position);

                        tiles.Add(new TileData()
                        {
                            TileAssetIndex = item.Value.AssetIndex,
                            Position = position,
                            TileBehaviorData = item.Value.RuntimeData,
                            WorldIndex = tiles.Count
                        });
                    }
                }

                var orderedTiles = tiles.OrderBy(x => x.Position.y).ThenBy(x => x.Position.x).ToArray();

                var levelData = new LevelTilesData(orderedTiles);

                _worlds[_selectedWorldIndex].LevelData = levelData;

                SaveChange();

            }
        }

        private void SaveChange()
        {
            var json = JsonConvert.SerializeObject(_worlds, typeof(BaseTD), Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            var worldLevelPath = Application.dataPath + "/Resources/Data/WorldData.txt";

            File.WriteAllText(worldLevelPath, json);

            Debug.Log(json);
        }

        private void ShowWorldTileData()
        {
            var tile = _tilemap.GetTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y, 0);
            if (tile != null)
            {
                GUILayout.BeginVertical();

                var texture = _tilesDatabase.GetTileTexture(tile.AssetIndex);

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
                        tile.RuntimeData = new StringDataTD();
                        break;
                }
            }

            return (BaseTD)tile.RuntimeData;
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
