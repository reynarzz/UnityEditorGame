using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DHierarchyEditor
    {
        private DEntitiesController _entitiesController;
        private Rect _rect = new Rect(0, 0, 200, 350);
        private Vector2 _scroll;

        public DHierarchyEditor()
        {
            _entitiesController = DIEngineCoreServices.Get<DEntitiesController>();
        }

        public void Update()
        {
            var entities = _entitiesController.GetAllGameEntities();

            GUILayout.BeginArea(_rect);

            var color = GUI.backgroundColor;
            GUI.backgroundColor = Color.black * 0.4f;
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = color;
            GUILayout.Label("Hierarchy");

            _scroll = GUILayout.BeginScrollView(_scroll);

            for (int i = 0; i < entities.Count; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal();
                entities[i].IsActive = EditorGUILayout.Toggle(entities[i].IsActive, GUILayout.MaxWidth(15));
                GUILayout.Label(entities[i].Name);
                GUILayout.EndHorizontal();

                DrawComponent(entities[i]);
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawComponent(DGameEntity entity)
        {
            var components = entity.GetAllComponents();

            GUILayout.BeginVertical();

            if (EditorGUILayout.Foldout(true, "Components"))
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);

                foreach (var component in components)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label(component.GetType().Name);

                    if (component.GetType() == typeof(DTransformComponent))
                    {
                        entity.Transform.Position = EditorGUILayout.Vector2Field(string.Empty, entity.Transform.Position, GUILayout.MaxWidth(_rect.width - 50));
                        entity.Transform.Scale = EditorGUILayout.Vector2Field(string.Empty, entity.Transform.Scale, GUILayout.MaxWidth(_rect.width - 50));
                    }
                    if (component.GetType() == typeof(DRendererComponent))
                    {
                        var comp = (component as DRendererComponent);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Color");
                        comp.Color = EditorGUILayout.ColorField(comp.Color);
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
                        GUILayout.Label("Debug");
                        box.Debug = EditorGUILayout.Toggle(box.Debug);
                        GUILayout.EndHorizontal();

                        box.Center = EditorGUILayout.Vector2Field(string.Empty, box.Center, GUILayout.MaxWidth(_rect.width - 50));
                        box.Size = EditorGUILayout.Vector2Field(string.Empty, box.Size, GUILayout.MaxWidth(_rect.width - 50));

                    }

                    if (component.GetType() == typeof(HealthBarUI))
                    {
                        var health = (component as HealthBarUI);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Percent", GUILayout.MaxWidth(40));
                        health.Percentage = EditorGUILayout.Slider(health.Percentage, 0, 1);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        health.Color = EditorGUILayout.ColorField(health.Color);
                        health.CutOffColor = EditorGUILayout.ColorField(health.CutOffColor);
                        GUILayout.EndHorizontal();

                    }

                    if (component.GetType() == typeof(ActorHealth))
                    {
                        var health = (component as ActorHealth);
                        GUILayout.Label("Health: " + health.Health);
                    }


                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

            }

            GUILayout.EndVertical();

        }
    }
}
