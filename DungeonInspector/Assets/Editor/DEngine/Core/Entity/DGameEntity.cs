using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DGameEntity
    {
        private Dictionary<Type, DComponent> _components;

        private DTransformComponent _transform;
        public DTransformComponent Transform => _transform;
        private List<DBehavior> _behaviorComponents_Test;

        private const string _defaultName = "GameEntity";
        public string Name { get; set; }

        public DGameEntity() : this(_defaultName) { }
        public DGameEntity(string name) : this(name, null) { }
        public DGameEntity(params Type[] components) : this(_defaultName, components) { }

        public DGameEntity(string name, params Type[] components)
        {
            Name = name;

            _transform = new DTransformComponent();

            _behaviorComponents_Test = new List<DBehavior>();

            _components = new Dictionary<Type, DComponent>()
            {
                { typeof(DTransformComponent), _transform }
            };

            if (components != null && components.Length > 0)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    AddComponent(components[i]);
                }
            }

            DIEngineCoreServices.Get<DEntitiesController>().AddEntity(this);
        }

        public DComponent AddComponent(Type type)
        {
            DComponent component = null;

            if (!_components.ContainsKey(type))
            {
                component = Activator.CreateInstance(type) as DComponent;

                if (type.IsSubclassOf(typeof(DTransformableComponent)))
                {
                    var updatable = component as DTransformableComponent;
                    updatable.Transform = Transform;

                }

                if (type.IsSubclassOf(typeof(DBehavior)))
                {
                    var behavior = component as DBehavior;

                    behavior.GameEntity = this;
                    _behaviorComponents_Test.Add(behavior);
                }

                if (type == typeof(DRendererComponent) || type.IsSubclassOf(typeof(DRendererComponent)))
                {
                    var renderer = component as DRendererComponent;
                    renderer.Transform = Transform;

                    DIEngineCoreServices.Get<DRenderingController>().AddRenderer(renderer);
                }

                // Temporal
                if (type == typeof(DCamera))
                {
                    DCamera.MainCamera = component as DCamera;
                }


                component.OnRemoved += OnComponentRemoved;

                _components.Add(type, component);
            }
            else
            {
                component = _components[type];
                Debug.LogError($"Already contains component of type {type.Name}");
            }

            return component;
        }

        public T AddComponent<T>() where T : DComponent, new()
        {
            return (T)AddComponent(typeof(T));
        }

        public T GetComponent<T>() where T : DComponent, new()
        {
            var type = typeof(T);

            if (_components.TryGetValue(type, out var component))
            {
                return component as T;
            }

            return default;
        }

        public bool TryGetComponent<T>(out T component) where T : DComponent, new()
        {
            component = GetComponent<T>();

            return component != null;
        }

        public List<DBehavior> GetAllUpdatableComponents()
        {
            return _behaviorComponents_Test;
        }

        private void OnComponentRemoved(DComponent component)
        {
            var updatableRenderer = component as DRendererComponent;

            if (updatableRenderer != null)
            {
                var result = DIEngineCoreServices.Get<DRenderingController>().RemoveRenderer(updatableRenderer);

            }

            var behavior = component as DBehavior;

            if (behavior != null)
            {
                _behaviorComponents_Test.Remove(behavior);

            }

            _components.Remove(component.GetType());

            // Removing

        }

        public void Destroy()
        {
            DIEngineCoreServices.Get<DEntitiesController>().RemoveEntity(this);

            for (int i = _components.Values.Count - 1; i > 0; i--)
            {
                _components.Values.ElementAt(i).Destroy();
            }

            _components.Clear();
        }
    }
}