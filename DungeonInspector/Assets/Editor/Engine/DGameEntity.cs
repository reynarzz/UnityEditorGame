using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DGameEntity
    {
        private Dictionary<Type, DComponent> _components;

        public DTransformComponent _transform;
        public DTransformComponent Transform => _transform;

        public string Name { get; set; } = "GameEntity";

        public DGameEntity()
        {
            _transform = new DTransformComponent();

            _components = new Dictionary<Type, DComponent>()
            {
                { typeof(DTransformComponent), _transform }
            };
        }

        public T AddComponent<T>() where T : DComponent, new()
        {
            var type = typeof(T);

            T component = default;

            if (!_components.ContainsKey(type))
            {
                component = new T();

                if(type == typeof(DBehaviorComponent))
                {
                    (component as DBehaviorComponent).GameEntity = this;
                }

                _components.Add(type, component);
            }
            else
            {
                component = _components[type] as T;
                Debug.LogError($"Already contains component of type {type.Name}");
            }

            return component;
        }

        public T GetComponent<T>() where T : DComponent, new()
        {
            var type = typeof(T);

            if (_components.TryGetValue(type, out var component))
            {
                return component as T;
            }
            else
            {
                Debug.LogError($"Doesn't contain component of type {type.Name}");
            }

            return default;
        }
    }
}