using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class GameEntity
    {
        private Dictionary<Type, DComponent> _components;

        private DTransformComponent _transform;
        public DTransformComponent Transform => _transform;
        private List<IDBehavior> _behaviorComponents_Test;

        private const string _defaultName = "GameEntity";
        public string Name { get; set; }

        public GameEntity() : this(_defaultName) { }
        public GameEntity(string name) : this(name, null) { }
        public GameEntity(params Type[] components) : this(_defaultName, components) { }

        public bool IsActive { get; set; } = true;

        public GameEntity(string name, params Type[] components)
        {
            Name = name;

            _transform = new DTransformComponent();

            _behaviorComponents_Test = new List<IDBehavior>();

            _components = new Dictionary<Type, DComponent>()
            {
                { typeof(DTransformComponent), _transform }
            };

            if (components != null && components.Length > 0)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    AddComp(components[i]);
                }
            }

            DIEngineCoreServices.Get<DEntitiesController>().Add(this);
        }

        // TODO: refactor
        public DComponent AddComp(Type type)
        {
            DComponent component = null;

            if (!_components.ContainsKey(type))
            {
                component = Activator.CreateInstance(type) as DComponent;
                component.Entity = this;

                if (type.IsSubclassOf(typeof(DTransformableComponent)))
                {
                    var updatable = component as DTransformableComponent;
                    updatable.Transform = Transform;

                }

                if (type == typeof(DRendererComponent) || type.IsSubclassOf(typeof(DRendererComponent)))
                {
                    var renderer = component as DRendererComponent;

                    DIEngineCoreServices.Get<DRenderingController>().Add(renderer);
                }

                // Temporal
                if (type == typeof(DCamera))
                {
                    DCamera.MainCamera = component as DCamera;
                }

                if (type.IsSubclassOf(typeof(DBehavior)))
                {
                    var behavior = component as DBehavior;

                    behavior.Entity = this;
                    _behaviorComponents_Test.Add(behavior);

                    //Debug.Log(component.GetType().Name);
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

        public T AddComp<T>() where T : DComponent, new()
        {
            return (T)AddComp(typeof(T));
        }

        public T GetComp<T>() where T : DComponent, new()
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
            component = GetComp<T>();

            return component != null;
        }

        public List<IDBehavior> GetAllUpdatableComponents()
        {
            return _behaviorComponents_Test;
        }

        private void OnComponentRemoved(DComponent component)
        {
            var updatableRenderer = component as DRendererComponent;

            if (updatableRenderer != null)
            {
                DIEngineCoreServices.Get<DRenderingController>().Remove(updatableRenderer);
            }

            var behavior = component as DBehavior;

            if (behavior != null)
            {
                _behaviorComponents_Test.Remove(behavior);

            }

            _components.Remove(component.GetType());

            // Removing
        }

        public static GameEntity FindGameEntity(string name)
        {
            return DIEngineCoreServices.Get<DEntitiesController>().FindGameEntity(name);
        }

        public void Destroy()
        {
            DIEngineCoreServices.Get<DEntitiesController>().Remove(this);

            for (int i = _components.Values.Count - 1; i > 0; i--)
            {
                _components.Values.ElementAt(i).Destroy();
            }

            _components.Clear();
        }
    }
}