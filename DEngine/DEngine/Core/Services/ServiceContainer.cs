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
        public virtual void OnPause() { }
        public virtual void OnResume() { }
        public virtual void OnGUI() { }
    }

    public abstract class DEngineSystemBase<T> : DEngineSystemBase
    {
        internal virtual void Add(T element) { }
        internal virtual void Remove(T element) { }
    }

    internal class DIEngineCoreServices
    {
        private static Dictionary<Type, DEngineSystemBase> _services;

        internal DIEngineCoreServices()
        {
            _services = new Dictionary<Type, DEngineSystemBase>()
            {
                { typeof(DTime), new DTime() },
                { typeof(DAudioSystem), new DAudioSystem() },
                { typeof(DInput), new DInput() },
                { typeof(DEntitiesController), new DEntitiesController() },
                { typeof(DPhysicsController), new DPhysicsController() },
                { typeof(DRendering), new DRendering() },
                { typeof(DEditorSystem), new DEditorSystem() },
                
            };
        }

        internal static T Get<T>() where T : DEngineSystemBase
        {
            return Get(typeof(T)) as T;
        }

        internal static DEngineSystemBase Get(Type type) 
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
