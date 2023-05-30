using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DEngineSystemBase 
    {
        public virtual void Init() { }
        public virtual void Update() { }
        public virtual void Cleanup() { }
    }

    public abstract class DEngineSystemBase<T> : DEngineSystemBase
    {
        public virtual void Add(T element) { }
        public virtual void Remove(T element) { }
    }

    internal class DIEngineCoreServices
    {
        private static Dictionary<Type, DEngineSystemBase> _services;

        public DIEngineCoreServices()
        {
            _services = new Dictionary<Type, DEngineSystemBase>()
            {
                { typeof(DTime), new DTime() },
                { typeof(DAudioSystem), new DAudioSystem() },
                { typeof(DInput), new DInput() },
                { typeof(DEntitiesController), new DEntitiesController() },
                { typeof(DRendering), new DRendering() },
                { typeof(DPhysicsController), new DPhysicsController() },
                { typeof(DEditorSystem), new DEditorSystem() },
                
            };
        }

        public static T Get<T>() where T : DEngineSystemBase
        {
            return Get(typeof(T)) as T;
        }

        public static DEngineSystemBase Get(Type type) 
        {
            if (_services.TryGetValue(type, out var service))
            {
                return service;
            }
            else
            {
                UnityEngine.Debug.LogError($"Cannot find service of type: {type}");
            }

            return default;
        }
    }
}
