using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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
        public class TilemapEditorInfo
        {
            public DTilemap Tilemap { get; set; }
            public bool Hidden { get; set; }
        }

        private Vector2 _scroll;
        private DTile _selectedTile;

        private TilemapEditorInfo _selectedTilemap;

        private List<TilemapEditorInfo> _tilemaps;

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

        private Color32 _selectedColor = new Color32(70, 96, 124, 255);

        private const float _mouseDeltaSens = 0.01557f;
        private List<WorldData> _worlds;

        private World _worldNameCreate;
        private static int _selectedWorldIndex;
        private static DVec2 _cameraPos;

        private EntityInfo _selectedEntity;
        private EditModePrefabInstantiator _entityPrefabList;
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
        private GUIContent _visibleIcon;
        private GUIContent _hiddenIcon;
        private GUIStyle _levelNameStyle;

        private bool _showManager = false;

        private DVec2 _pickerScroll;
        private int _selectedIndex = 0;
        private ReorderableList _tilemapReorderableList;

        private enum TileDatabaseType
        {
            Static,
            Animated,
            Interactable,
        }

        protected override void OnStart()
        {

            _tilesDatabase = new TilesDatabase("World/World1Tiles");
            _animatedTiles = new TilesDatabase("World/TilesAnimated");
            _entityPrefabList = new EditModePrefabInstantiator();

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
            _visibleIcon = EditorGUIUtility.IconContent("d_scenevis_visible_hover");
            _hiddenIcon = EditorGUIUtility.IconContent("d_scenevis_hidden_hover");



            _levelNameStyle = new GUIStyle(GUI.skin.button);
            _levelNameStyle.alignment = TextAnchor.MiddleLeft;
            _levelNameStyle.contentOffset = new Vector2(3, 0);

            _camera = DGameEntity.FindGameEntity("MainCamera").GetComp<DCamera>();

            _selectedTile = _tilesDatabase.GetTile(0);

            _tilesBrushIcons = new GUIContent[_tilesDatabase.Count];
            _placedEntities = new List<(DVec2, EntityInfo)>();

            for (int i = 0; i < _tilesBrushIcons.Length; i++)
            {
                var tile = _tilesDatabase.GetTile(i);

                _tilesBrushIcons[i] = new GUIContent(tile.Texture, tile.TextureName);
            }

            _currentBrushIcons = _tilesBrushIcons;

            _brushModes = new GUIContent[] { _selecticon, _brushIcon, _eraserIcon };

            var data = Resources.Load<TextAsset>("Data/WorldData");

            AssetDatabase.Refresh();


            _tilemaps = new List<TilemapEditorInfo>();

            if (data != null)
            {
                _worlds = JsonConvert.DeserializeObject<List<WorldData>>(data.text,
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                if (_worlds.Count > _selectedWorldIndex)
                {
                    var world = _worlds[_selectedWorldIndex];

                    Load(world);
                }
            }
            else
            {
                _worlds = new List<WorldData>();
                _selectedTilemap = GetNewTilemap("Tilemap", 0);

                _tilemaps.Add(_selectedTilemap);
            }

            _tilemapReorderableList = new ReorderableList(_tilemaps, typeof(DTilemap));

            _tilemapReorderableList.headerHeight = 0;
            _tilemapReorderableList.footerHeight = 0;
            _tilemapReorderableList.displayAdd = false;
            _tilemapReorderableList.displayRemove = false;
            _tilemapReorderableList.multiSelect = true;
            _tilemapReorderableList.drawElementCallback = OnDrawLayer;
            _tilemapReorderableList.onReorderCallback = OnReorderedList;
            _tilemapReorderableList.index = 0;

            _tilemapReorderableList.drawNoneElementCallback = OnDrawNoLayer;


            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");
            _selectionFrame = Resources.Load<Texture2D>("GameAssets/LevelEditor/SelectionFrame");

            _entityBrushIcons = new GUIContent[_entityPrefabList.Count];

            for (int i = 0; i < _entityPrefabList.Count; i++)
            {
                var tex = _entityPrefabList.GetEntityInfo((EntityID)i).Tex;

                _entityBrushIcons[i] = new GUIContent(tex, tex.name);
            }
        }

        private void OnReorderedList(ReorderableList list)
        {
            for (int i = 0; i < _tilemaps.Count; i++)
            {
                var solvedIndex = _tilemaps.Count - 1 - i;

                _tilemaps[i].Tilemap.GetComp<DTilemapRendererComponent>().ZSorting = solvedIndex;
            }
        }

        private TilemapEditorInfo GetNewTilemap(string name, int sorting)
        {
            var tilemapObj = new DGameEntity(name);
            var tilemap = tilemapObj.AddComp<DTilemap>();

            var renderer = tilemapObj.AddComp<DTilemapRendererComponent>();

            renderer.TileMap = tilemap;
            renderer.ZSorting = sorting;

            return new TilemapEditorInfo() { Tilemap = tilemap };
        }

        private void OnDrawNoLayer(Rect rect)
        {
            GUI.Label(rect, "No Layers");
        }

        private void OnDrawLayer(Rect rect, int index, bool isActive, bool isFocused)
        {
            var startWidth = rect.width;
            var startY = rect.y;

            rect.width = 22;
            rect.y += 2;


            var tilemap = _tilemaps[index];

            var solvedIndex = _tilemaps.Count - index - 1;


            var visibilityIcon = tilemap.Hidden ? _hiddenIcon : _visibleIcon;

            if (GUI.Button(rect, visibilityIcon, GUIStyle.none))
            {
                tilemap.Hidden = !tilemap.Hidden;

                tilemap.Tilemap.Enabled = !tilemap.Hidden;
            }


            rect.y = startY;

            rect.x += rect.width;
            rect.width = startWidth - rect.width;
            GUI.Label(rect, $"{tilemap.Tilemap.Name} {solvedIndex}");
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
            else if (_selectedEntity != null)
            {
                _mouseTileGuidePosition = new Vector2Int(Mathf.RoundToInt(newMousePos.x), Mathf.RoundToInt(newMousePos.y));

                if (DInput.IsMouse(0))
                {

                }
            }
            DCamera.PixelSize += (int)DInput.MouseWheelDelta.y * -1;

            DCamera.PixelSize = Mathf.Clamp(DCamera.PixelSize, 16, 120);

            var tex = default(Texture2D);

            var isInside = _camera.IsInside(newMousePos, Vector2.one * 0.01f);

            if (isInside)
            {
                if (DInput.IsMouse(0) && BrushMode == BrushMode.Paint)
                {
                    if (_paintType == PaintType.Tile)
                    {
                        _selectedTilemap.Tilemap.SetNewTile(_selectedTile, _mouseTileGuidePosition.x, _mouseTileGuidePosition.y);

                    }
                    else if (_paintType == PaintType.Entity)
                    {
                        if (!_placedEntities.Exists(x => x.Item1.RoundToInt() == _mouseTileGuidePosition && x.Item2.EntityID == _selectedEntity.EntityID))
                        {
                            _placedEntities.Add((_mouseTileGuidePosition, _selectedEntity));
                        }
                        else
                        {
                            var index = _placedEntities.FindIndex(x => x.Item1.RoundToInt() == _mouseTileGuidePosition);

                            _placedEntities[index] = (_mouseTileGuidePosition, _selectedEntity);
                        }
                    }
                }
                else if ((DInput.IsMouse(0) || DInput.IsMouse(1)) && BrushMode == BrushMode.Eraser)
                {
                    _selectedTilemap.Tilemap.RemoveTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
                    _selectedTilemap.Tilemap.RecalculateBounds();
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

            if (_paintType == PaintType.Tile)
            {

                tex = BrushMode == BrushMode.Paint ? _selectedTile.Texture : _selectionFrame;
            }
            else if (_paintType == PaintType.Entity)
            {
                tex = BrushMode == BrushMode.Paint ? _selectedEntity.Tex : _selectionFrame;
            }
            //// Mouse sprite pointer
            //DrawSprite(newMousePos, Vector2.one, _camera_Test, WorldEditorEditor.SelectedTex);

            Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), tex, _mat_DELETE);

            if (_selectedEntity != null)
            {
                var width = _selectedEntity.Tex.width;
                var height = _selectedEntity.Tex.height;

                Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, new DVec2(width, height) * 0.064f), _selectedEntity.Tex, _mat_DELETE);
            }

            if (_placedEntities.Count > 0)
            {
                for (int i = 0; i < _placedEntities.Count; i++)
                {
                    var entTex = _placedEntities[i].Item2.Tex;

                    var width = entTex.width;
                    var height = entTex.height;

                    Graphics.DrawTexture(_camera.World2RectPos(_placedEntities[i].Item1, new DVec2(width, height) * 0.064f), entTex, _mat_DELETE);
                }
            }
            //Graphics.DrawTexture(_camera.World2RectPos(_mouseTileGuidePosition, Vector2.one), _selectionFrame, _mat_DELETE);
        }

        protected override void OnGUI()
        {
            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label);
                _style.fontSize = 20;
            }
            GUILayout.Space(30);


            GUILayout.Label(_mouseTileGuidePosition.ToString());

            //TilesPicker();

            if (true)
            {
                GUILayout.BeginArea(new Rect(EditorGUIUtility.currentViewWidth - 250, 40, 240, 350), EditorStyles.helpBox);


                GUILayout.Space(3);
                GUILayout.BeginHorizontal();


                GUILayout.BeginVertical();
                {

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
                }

                GUILayout.EndVertical();

                //GUILayout.FlexibleSpace();
                _selectedIndex = DrawPallete(_currentBrushIcons, _selectedIndex, _currentBrushIcons.Length);

                //PickEntity(index);
                //GUILayout.FlexibleSpace();

                DrawTilemapSelector();

                GUILayout.EndHorizontal();


                if (_paintType == PaintType.Tile)
                {
                    _selectedEntity = null;
                    _selectedTile = _tilesDatabase.GetTile(_selectedIndex);
                }
                else if (_paintType == PaintType.Entity)
                {
                    _selectedTile = null;
                    _selectedEntity = _entityPrefabList.GetEntityInfo((EntityID)_selectedIndex);
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


            Utils.DrawBounds(_selectedTilemap.Tilemap.GetTilemapBoundaries(), Color.white, 0.5f);

            MousePointer();
        }

        private void PickEntity(int index)
        {
            if (index >= 0 && index < _entityPrefabList.Count)
            {
                _selectedEntity = _entityPrefabList.GetEntityInfo((EntityID)index);
            }
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

        private int DrawPallete(GUIContent[] items, int selected, int max)
        {
            GUILayout.BeginVertical();
            _pickerScroll = GUILayout.BeginScrollView(_pickerScroll, false, false, GUIStyle.none, GUIStyle.none, EditorStyles.helpBox, GUILayout.MinWidth(43));

            if (selected > max)
            {
                selected = 0;
            }

            for (int i = 0; i < items.Length; i++)
            {
                var info = items[i];

                var c = GUI.backgroundColor;

                if (selected == i)
                {
                    GUI.backgroundColor = new Color(0.5f, 0.8f, 1, 1);
                }

                if (GUILayout.Button(info, GUILayout.MaxWidth(35), GUILayout.MinHeight(35)))
                {
                    selected = i;
                }

                GUI.backgroundColor = c;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            return selected;
        }




        private Vector2 _levelManagerScroll;

        private void LevelManager()
        {
            GUILayout.BeginArea(new Rect(210, 40, EditorGUIUtility.currentViewWidth - 465, 350), EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_closeIcon, GUIStyle.none))
            {
                _showManager = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("New", GUILayout.Width(30));
            _worldNameCreate = (World)EditorGUILayout.EnumPopup(_worldNameCreate);

            if (GUILayout.Button(_addIcon, GUILayout.Width(25), GUILayout.Height(25)))
            {
                if (!_worlds.Exists(x => x.World == _worldNameCreate))
                {
                    if (EditorUtility.DisplayDialog("Create Level", $"Want to Create '{_worldNameCreate}' level?", "Ok", "Cancel"))
                    {
                        var tilemap = GetNewTilemap("Tilemap", 0);

                        var newWorld = new WorldData() { World = _worldNameCreate, TilemapsData = new TilemapData[] { new TilemapData(new TileData[0]) } };

                        _worlds.Add(newWorld);

                        _selectedWorldIndex = _worlds.Count - 1;
                        Load(newWorld);
                        OnSave();

                        _worldNameCreate = default;
                    }

                }
                else
                {
                    Debug.Log("This world exist already!");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            _levelManagerScroll = GUILayout.BeginScrollView(_levelManagerScroll, EditorStyles.helpBox);

            for (int i = 0; i < _worlds.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(_worlds[i].World.ToString(), _levelNameStyle, GUILayout.Height(25)))
                {
                    _selectedWorldIndex = i;
                    _showManager = false;
                    Load(_worlds[i]);
                }

                if (GUILayout.Button(_trashIcon, GUILayout.Width(25), GUILayout.Height(25)))
                {
                    if (EditorUtility.DisplayDialog("Delete", $"Want to delete '{_worlds[i].World}' level?", "Ok", "Cancel"))
                    {
                        _worlds.RemoveAt(i);
                        _selectedWorldIndex = 0;
                        _selectedTilemap.Tilemap.Clear();
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

        private void Load(WorldData worldData)
        {
            if (_tilemaps.Count > 0)
            {
                _tilemaps.ForEach(x => x.Tilemap.Entity.Destroy());
                _tilemaps.Clear();
            }

            _placedEntities.Clear();

            if (worldData != null)
            {
                if (worldData.TilemapsData != null)
                {
                    for (int i = 0; i < worldData.TilemapsData.Length; i++)
                    {
                        var tilemap = GetNewTilemap("Tilemap", worldData.TilemapsData.Length - i - 1);

                        for (int j = 0; j < worldData.TilemapsData[i].Count; j++)
                        {
                            var info = worldData.TilemapsData[i].GetTile(j);

                            tilemap.Tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
                        }

                        _tilemaps.Add(tilemap);
                    }


                }

                //var tilemap = GetNewTilemap("Tilemap");
                //_tilemaps.Add(tilemap);
                //for (int j = 0; j < worldData.LevelData.Count; j++)
                //{
                //    var info = worldData.LevelData.GetTile(j);

                //    tilemap.Tilemap.SetTile(_tilesDatabase.GetNewTile(info), info.Position.x, info.Position.y);
                //}

                _selectedTilemap = _tilemaps[0];

                for (int i = 0; i < worldData.Entities.Count; i++)
                {
                    var ent = worldData.Entities[i];

                    var info = _entityPrefabList.GetEntityInfo(ent.Item1);
                    _placedEntities.Add((ent.Item2, info));

                }
            }

            _selectedTilemap.Tilemap.RecalculateBounds();
        }

        private void OnSave()
        {
            //if (_tilemap.Tiles.Count > 0)
            {
                var world = _worlds[_selectedWorldIndex];

                world.TilemapsData = new TilemapData[_tilemaps.Count];

                for (int i = 0; i < _tilemaps.Count; i++)
                {
                    var tilemapInfo = _tilemaps[i];

                    var tilesLength = tilemapInfo.Tilemap.Tiles.Count;

                    var tiles = new TileData[tilesLength];

                    for (int j = 0; j < tilesLength; j++)
                    {
                        var tile = tilemapInfo.Tilemap.Tiles.ElementAt(j);

                        var position = tile.Key;

                        tiles[j] = new TileData()
                        {
                            TileAssetIndex = tile.Value.AssetIndex,
                            Position = position,
                            TileBehaviorData = tile.Value.RuntimeData
                        };
                    }

                    var orderedTiles = tiles.OrderBy(x => x.Position.y).ThenBy(x => x.Position.x).ToArray();

                    // Sets worldIndex
                    for (int j = 0; j < orderedTiles.Length; j++)
                    {
                        orderedTiles[j].WorldIndex = j;
                    }

                    world.TilemapsData[i] = new TilemapData(orderedTiles);
                }

                world.Entities = _placedEntities.Select(x => (x.Item2.EntityID, x.Item1)).ToList();

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
            var tile = _selectedTilemap.Tilemap.GetTile(_mouseTileGuidePosition.x, _mouseTileGuidePosition.y);
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
                        tile.RuntimeData = new SpikeFloorTD();
                        break;
                    case TileBehavior.IncreaseHealth:
                        break;
                    case TileBehavior.ChangeLevel:
                        tile.RuntimeData = new ChangeLevelTD();
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

                GUILayout.BeginVertical(EditorStyles.helpBox);
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
                            GUILayout.Label(name, GUILayout.MinWidth(100));
                            value = EditorGUILayout.FloatField((float)value);
                            GUILayout.EndHorizontal();
                        }
                        else if (propType.IsAssignableFrom(typeof(int)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name, GUILayout.MinWidth(100));
                            value = EditorGUILayout.IntField((int)value);
                            GUILayout.EndHorizontal();
                        }
                        else if (propType.IsAssignableFrom(typeof(DVec2)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name, GUILayout.MinWidth(100));
                            value = (DVec2)EditorGUILayout.Vector2IntField(string.Empty, (DVec2)value);
                            GUILayout.EndHorizontal();
                        }
                        else if (propType.IsEnum)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name, GUILayout.MinWidth(100));
                            value = EditorGUILayout.EnumPopup((Enum)value);
                            GUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        if (propType.IsAssignableFrom(typeof(string)))
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(name, GUILayout.MinWidth(100));
                            value = EditorGUILayout.TextField((string)value);
                            GUILayout.EndHorizontal();
                        }

                    }
                    property.SetValue(type, value);
                }
                GUILayout.EndVertical();
            }
        }

        private Vector2 _layerScrollView;

        private void DrawTilemapSelector()
        {
            if (_tilemaps.Count <= _tilemapReorderableList.index)
            {
                _tilemapReorderableList.index = _tilemaps.Count - 1;
            }

            if (_tilemaps.Count > 0)
            {
                _selectedTilemap = _tilemaps[_tilemapReorderableList.index];
            }

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(140));

            GUILayout.Label($"Tilemaps ({_selectedTilemap?.Tilemap.Name + (_tilemapReorderableList.count - 1 - _tilemapReorderableList.index) ?? "None"})");
            _layerScrollView = GUILayout.BeginScrollView(_layerScrollView);
            _tilemapReorderableList.DoLayoutList();

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_addIcon))
            {
                var tilemap = GetNewTilemap("Tilemap", _tilemaps.Count - 1);

                _tilemaps.Insert(0, tilemap);
            }

            if (_tilemaps.Count > 1 && GUILayout.Button(_trashIcon))
            {
                var descending = _tilemapReorderableList.selectedIndices.OrderByDescending(x => x);

                foreach (var item in descending)
                {
                    _tilemaps[item].Tilemap.Entity.Destroy();
                    _tilemaps.RemoveAt(item);
                }

                _selectedTilemap = _tilemaps.FirstOrDefault();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
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
