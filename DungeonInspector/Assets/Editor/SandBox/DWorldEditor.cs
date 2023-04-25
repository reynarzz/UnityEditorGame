using AStar.Collections.MultiDimensional;
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
    public enum BrushMode
    {
        Select,
        Paint,
        Eraser
    }

    public enum PaintType
    {
        Tile,
        Entity
    }

    public class DWorldEditor : DBehavior
    {
        private Vector2 _scroll;
        private DTile _selectedTile;

        private DTilemap _tilemap;

        private TilesDatabase _tilesDatabase;
        private TilesDatabase _animatedTiles;
        private DCamera _camera;

        public BrushMode BrushMode { get; private set; } = BrushMode.Select;
        private PaintType _paintType = PaintType.Tile;

        private GUIContent[] _brushModes;

        private static Vector2Int _mouseTileGuidePosition;
        private Material _mat_DELETE;
        private Texture2D _selectionFrame;
        private GUIStyle _style;


        private const float _mouseDeltaSens = 0.01557f;
        private List<WorldData> _worlds;

        private string _worldNameCreate;
        private static int _selectedWorldIndex;
        private static DVec2 _cameraPos;

        private EntityInfo _currentPickedEntity;
        private EditModePrefabInstantiator _entityList;
        private GUIContent[] _entityBrushIcons;
        private GUIContent[] _tilesBrushIcons;

        private GUIContent[] _currentBrushIcons;

        private List<(DVec2, EntityInfo)> _placedEntities;

        private GUIContent _brushIcon;
        private GUIContent _eraserIcon;
        private GUIContent _selecticon;
        private GUIContent _folderIcon;
        private GUIContent _saveIcon;
        private GUIContent _entityIcon;
        private GUIContent _tileIcon;
        private GUIContent _trashIcon;
        private GUIContent _addIcon;
        private GUIContent _closeIcon;

        private GUIStyle _levelNameStyle;

        private bool _showManager = false;

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

            _brushIcon = EditorGUIUtility.IconContent("ClothInspector.PaintTool");
            _eraserIcon = EditorGUIUtility.IconContent("d_Grid.EraserTool");
            _selecticon = EditorGUIUtility.IconContent("d_Grid.BoxTool");
            _folderIcon = EditorGUIUtility.IconContent("d_FolderEmpty Icon");
            _saveIcon = EditorGUIUtility.IconContent("d_SaveAs");
            _entityIcon = EditorGUIUtility.IconContent("d_GameObject Icon");
            _tileIcon = EditorGUIUtility.IconContent("d_Tile Icon");
            _trashIcon = EditorGUIUtility.IconContent("TreeEditor.Trash");
            _addIcon = EditorGUIUtility.IconContent("CreateAddNew");
            _closeIcon = EditorGUIUtility.IconContent("d_winbtn_win_close");

            _levelNameStyle = new GUIStyle(GUI.skin.button);
            _levelNameStyle.alignment = TextAnchor.MiddleLeft;
            _levelNameStyle.contentOffset = new Vector2(3, 0);
            _tilemap = tilemap;

            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _selectedTile = _tilesDatabase.GetTile(0);

            _tilesBrushIcons = new GUIContent[_tilesDatabase.Count];
            for (int i = 0; i < _tilesBrushIcons.Length; i++)
            {
                var tile = _tilesDatabase.GetTile(i);

                _tilesBrushIcons[i] = new GUIContent(tile.Texture, tile.TextureName);
            }

            _currentBrushIcons = _tilesBrushIcons;

            _brushModes = new GUIContent[] { _selecticon, _brushIcon, _eraserIcon };

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

            _entityBrushIcons = new GUIContent[_entityList.Count];

            for (int i = 0; i < _entityList.Count; i++)
            {
                var tex = _entityList.GetEntityInfo(i).Tex;

                _entityBrushIcons[i] = new GUIContent(tex, tex.name);
            }
        }

        private void MousePointer()
        {
            var mouse = Event.current;

            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);

            if (BrushMode != BrushMode.Select)
            {
                _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

                if (DInput.IsMouseDown(0))
                {
                    BrushMode = BrushMode.Paint;
                }
                else if (DInput.IsMouseDown(1))
                {
                    BrushMode = BrushMode.Eraser;
                }
            }
            else if (_currentPickedEntity != null)
            {
                _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

                if (DInput.IsMouse(0))
                {

                }
            }

            var tex = default(Texture2D);

            var isInside = _camera.IsInside(newMousePos, Vector2.one * 0.01f);

            if (isInside)
            {
                if (DInput.IsMouse(0) && BrushMode == BrushMode.Paint)
                {
                    _tilemap.SetNewTile(_selectedTile, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                }
                else if ((DInput.IsMouse(0) || DInput.IsMouse(1)) && BrushMode == BrushMode.Eraser)
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
            tex = BrushMode == BrushMode.Paint ? _selectedTile.Texture : _selectionFrame;

            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);

            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), tex, _mat_DELETE);

            if (_currentPickedEntity != null)
            {
                var width = _currentPickedEntity.Tex.width;
                var height = _currentPickedEntity.Tex.height;

                Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, new DVec2(width, height) * 0.064f), _currentPickedEntity.Tex, _mat_DELETE);
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

            MousePointer();


            GUILayout.Label(_mouseTileGuidePosition.ToString());

            //TilesPicker();

            if (true)
            {
                GUILayout.BeginArea(new Rect(EditorGUIUtility.currentViewWidth - 110, 40, 90, 350), EditorStyles.helpBox);


                GUILayout.Space(3);
                GUILayout.BeginHorizontal();


                GUILayout.BeginVertical();
                if (GUILayout.Button(_tileIcon, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    Event.current.Use();

                    _paintType = PaintType.Tile;
                    _currentBrushIcons = _tilesBrushIcons;

                }

                if (GUILayout.Button(_entityIcon, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    Event.current.Use();

                    _paintType = PaintType.Entity;
                    _currentBrushIcons = _entityBrushIcons;
                }

                EditorGUILayout.Separator();

                DrawBrushModes();


                //Handles.DrawDottedLine(new Vector3(0, 0, 0), new Vector3(15, 15, 0), 5);

                EditorGUILayout.Separator();

                if (GUILayout.Button(_saveIcon, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    Event.current.Use();

                    OnSave();
                }


                if (GUILayout.Button(_folderIcon, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    Event.current.Use();

                    _showManager = true;
                }


                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
                var index = DrawPallete(_currentBrushIcons);
                GUILayout.EndHorizontal();



                if (index >= 0 && index < _entityList.Count)
                {
                    _currentPickedEntity = _entityList.GetEntityInfo(index);
                }
                GUILayout.Space(3);

                GUILayout.EndArea();
            }

            if (BrushMode == BrushMode.Select)
            {
                ShowWorldTileData();
            }

            if (_showManager)
            {
                LevelManager();
            }


            Utils.DrawBounds(_tilemap.GetTilemapBoundaries(), Color.white, 0.5f);
        }

        private void DrawBrushModes()
        {
            for (int i = 0; i < _brushModes.Length; i++)
            {
                if (GUILayout.Button(_brushModes[i], GUILayout.Width(30), GUILayout.Height(30)))
                {
                    BrushMode = (BrushMode)i;
                }
            }
        }

        private DVec2 _pickerScroll;
        private int _selectedEntity = -1;

        private int DrawPallete(GUIContent[] items)
        {
            GUILayout.BeginVertical();
            _pickerScroll = GUILayout.BeginScrollView(_pickerScroll, false, false, GUIStyle.none, GUIStyle.none, EditorStyles.helpBox, GUILayout.MinWidth(43));

            for (int i = 0; i < items.Length; i++)
            {
                var info = items[i];

                var c = GUI.backgroundColor;

                if (_selectedEntity == i)
                {
                    GUI.backgroundColor = new Color(0.4f, 0.8f, 1, 1);
                }

                if (GUILayout.Button(info, GUILayout.MaxWidth(35), GUILayout.MinHeight(35)))
                {
                    _selectedEntity = i;
                }

                GUI.backgroundColor = c;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            return _selectedEntity;
        }




        private Vector2 _levelManagerScroll;

        private void LevelManager()
        {
            GUILayout.BeginArea(new Rect(210, 40, EditorGUIUtility.currentViewWidth - 325, 350), EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_closeIcon, GUIStyle.none))
            {
                _showManager = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("New", GUILayout.Width(30));
            _worldNameCreate = GUILayout.TextField(_worldNameCreate);

            if (GUILayout.Button(_addIcon, GUILayout.Width(25), GUILayout.Height(25)))
            {
                if (!string.IsNullOrEmpty(_worldNameCreate))
                {
                    if (EditorUtility.DisplayDialog("Create Level", $"Want to Create '{_worldNameCreate}' level?", "Ok", "Cancel"))
                    {
                        _worlds.Add(new WorldData() { Name = _worldNameCreate });

                        _worldNameCreate = null;

                        _selectedWorldIndex = _worlds.Count - 1;
                        _tilemap.Clear();
                        OnSave();
                    }

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
                if (GUILayout.Button(_worlds[i].Name, _levelNameStyle, GUILayout.Height(25)))
                {
                    _selectedWorldIndex = i;
                    _showManager = false;
                    Load(_worlds[i].LevelData);
                }

                if (GUILayout.Button(_trashIcon, GUILayout.Width(25), GUILayout.Height(25)))
                {
                    if (EditorUtility.DisplayDialog("Delete", $"Want to delete '{_worlds[i].Name}' level?", "Ok", "Cancel"))
                    {
                        _worlds.RemoveAt(i);
                        _selectedWorldIndex = 0;
                        _tilemap.Clear();
                        SaveChange();
                        GUILayout.EndHorizontal();
                        break;
                    }

                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        private void Load(LevelTilesData levelData)
        {
            _tilemap.Clear();

            if (levelData != null)
            {
                for (int i = 0; i < levelData.Count; i++)
                {
                    var info = levelData.GetTile(i);

                    _tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
                }
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
