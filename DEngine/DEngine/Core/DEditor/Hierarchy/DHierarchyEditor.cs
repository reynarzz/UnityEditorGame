using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DHierarchyEditor
    {
        private DEntitiesController _entitiesController;
        private DRenderingController _renderingController;
        private Rect _rect = new Rect(0, 30, 130, 101);
        private Vector2 _scroll;
        private bool _show = false;

        private bool[] _foldOut;
        private bool _debugAll;

        private GUIContent[] _playModes;
        private int _playModeSelected = -1;
     
        public DHierarchyEditor()
        {
            // test
            _foldOut = new bool[50];
            _entitiesController = DIEngineCoreServices.Get<DEntitiesController>();
            _renderingController = DIEngineCoreServices.Get<DRenderingController>();
        }

        public void Update()
        {
            var entities = _entitiesController.GetAllGameEntities();

            _rect.width = Mathf.Lerp(_rect.width, (_show ? 200 : 130), DTime.DeltaTime * 10);
            _rect.height = Mathf.Lerp(_rect.height, (_show ? 350 : 101), DTime.DeltaTime * 17);

            GUILayout.BeginArea(_rect);
            EditorGUI.DrawRect(_rect, new Color(0.15f, 0.15f, 0.15f, 0.9f));

            var color = GUI.backgroundColor;
            GUI.backgroundColor = Color.black * 0.4f;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = color;

            GUILayout.BeginHorizontal();
            _show = EditorGUILayout.Toggle(_show, GUILayout.MaxWidth(15));
            GUILayout.Label("Hierarchy");
            GUILayout.EndHorizontal();

       

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label($"FPS: {DTime.FPs}");
            GUILayout.EndVertical();
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            _renderingController.V2Rendering = EditorGUILayout.Toggle(_renderingController.V2Rendering, GUILayout.MaxWidth(15));
            GUILayout.Label("V2 Rendering", GUILayout.MaxWidth(160));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            _debugAll = EditorGUILayout.Toggle(_debugAll, GUILayout.MaxWidth(15));
            GUILayout.Label("Debug All", GUILayout.MaxWidth(60));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            //if (_show)
            {
                _scroll = GUILayout.BeginScrollView(_scroll);

                for (int i = 0; i < entities.Count; i++)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    entities[i].IsActive = EditorGUILayout.Toggle(entities[i].IsActive, GUILayout.MaxWidth(15));
                    //GUILayout.Label(entities[i].Name, GUILayout.MaxWidth(120));
                    GUILayout.Space(10);
                    _foldOut[i] = EditorGUILayout.Foldout(_foldOut[i], entities[i].Name, true);
                    GUILayout.EndHorizontal();

                    if (_debugAll)
                    {
                        _foldOut[i] = _debugAll;
                    }

                    if (_foldOut[i])
                    {
                        DrawComponent(entities[i]);
                    }

                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();

            }

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        private void DrawComponent(DGameEntity entity)
        {
            var components = entity.GetAllComponents();


            GUILayout.BeginVertical();

            var show = EditorGUILayout.Foldout(true, "Components");

            if (_debugAll)
            {
                show = _debugAll;
            }

            if (show)
            {
                GUILayout.BeginVertical(/*EditorStyles.helpBox*/);

                foreach (var component in components)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);

                    GUILayout.Label(component.GetType().Name, GUILayout.MaxWidth(130));


                    if (component.GetType() == typeof(DCamera))
                    {
                        var comp = (component as DCamera);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Size", GUILayout.MaxWidth(40));
                        DCamera.PixelSize = (int)EditorGUILayout.Slider(DCamera.PixelSize, 16, 64);
                        GUILayout.EndHorizontal();
                    }

                    if (component.GetType() == typeof(DTransformComponent))
                    {
                        entity.Transform.Position = EditorGUILayout.Vector2Field(string.Empty, entity.Transform.Position, GUILayout.MaxWidth(_rect.width - 50));
                        entity.Transform.Scale = EditorGUILayout.Vector2Field(string.Empty, entity.Transform.Scale, GUILayout.MaxWidth(_rect.width - 50));
                    }

                    if (component.GetType() == typeof(DRendererComponent) || component is DRendererUIComponent)
                    {
                        var comp = (component as DRendererComponent);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Color");
                        comp.Color = EditorGUILayout.ColorField(comp.Color);
                        GUILayout.EndHorizontal();
                    }

                    if (component is DRendererAtlasComponent)
                    {
                        var atlas = component as DRendererAtlasComponent;

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Coord");
                        atlas.SpriteCoord = EditorGUILayout.Vector2IntField(string.Empty, atlas.SpriteCoord, GUILayout.MaxWidth(_rect.width - 50));
                        GUILayout.EndHorizontal();
                    }

                    if (component.GetType() == typeof(DBoxCollider))
                    {
                        var box = (component as DBoxCollider);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("IsTrigger");
                        box.IsTrigger = EditorGUILayout.Toggle(box.IsTrigger);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        //  GUILayout.Label("Debug");
                        //--   if (_debugAll)
                        box.Debug = _debugAll;// EditorGUILayout.Toggle(box.Debug);
                        GUILayout.EndHorizontal();

                        box.Center = EditorGUILayout.Vector2Field(string.Empty, box.Center, GUILayout.MaxWidth(_rect.width - 50));
                        box.Size = EditorGUILayout.Vector2Field(string.Empty, box.Size, GUILayout.MaxWidth(_rect.width - 50));

                    }

                    var fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    foreach (var field in fields)
                    {
                        var attrib = field.GetCustomAttribute<DExposeSlider>();

                        var expose = field.GetCustomAttribute<DExposeAttribute>();

                        if (expose != null)
                        {
                            RenderField(component, field);
                        }

                        if (attrib != null)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(field.Name);
                            field.SetValue(component, EditorGUILayout.Slider((float)field.GetValue(component), (float)attrib.Min, (float)attrib.Max));
                            GUILayout.EndHorizontal();
                        }
                    }
                    //if (component.GetType() == typeof(HealthBarUI))
                    //{
                    //    var health = (component as HealthBarUI);
                    //    GUILayout.BeginHorizontal();
                    //    GUILayout.Label("Percent", GUILayout.MaxWidth(40));
                    //    health.Percentage = EditorGUILayout.Slider(health.Percentage, 0, 1);
                    //    GUILayout.EndHorizontal();

                    //    GUILayout.BeginHorizontal();
                    //    health.Color = EditorGUILayout.ColorField(health.Color);
                    //    health.CutOffColor = EditorGUILayout.ColorField(health.CutOffColor);
                    //    GUILayout.EndHorizontal();

                    //}

                    //if (component.GetType() == typeof(ActorHealth))
                    //{
                    //    var health = (component as ActorHealth);
                    //    GUILayout.Label("Health: " + health.Health);
                    //}


                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

            }

            GUILayout.EndVertical();

        }

        private void RenderField(object target, FieldInfo field)
        {
            if (field != null)
            {
                var name = field.Name;
                var value = field.GetValue(target);
                var fieldType = field.FieldType;

                if (fieldType.IsValueType)
                {
                    if (fieldType == typeof(float))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(name);
                        value = EditorGUILayout.FloatField((float)value);
                        GUILayout.EndHorizontal();
                    }
                    else if (fieldType == typeof(int))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(name);
                        value = EditorGUILayout.IntField((int)value);
                        GUILayout.EndHorizontal();
                    }
                    else if (fieldType == typeof(bool))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(name);
                        value = EditorGUILayout.Toggle((bool)value);
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    if (fieldType == typeof(string))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(name);
                        value = EditorGUILayout.TextField((string)value);
                        GUILayout.EndHorizontal();
                    }
                }

                field.SetValue(target, value);
            }
        }
    }
}
