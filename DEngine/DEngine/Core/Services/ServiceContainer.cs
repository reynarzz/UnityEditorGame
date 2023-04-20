using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class EngineSystemBase 
    {
        public virtual void Init() { }
        public virtual void Update() { }
        public virtual void Cleanup() { }
    }

    public abstract class EngineSystemBase<T> : EngineSystemBase
    {
        public virtual void Add(T element) { }
        public virtual void Remove(T element) { }
    }

    public class DIEngineCoreServices
    {
        private static Dictionary<Type, EngineSystemBase> _services;

        public DIEngineCoreServices()
        {
            _services = new Dictionary<Type, EngineSystemBase>()
            {
                { typeof(DTime), new DTime() },
                { typeof(DAudioSystem), new DAudioSystem() },
                { typeof(DInput), new DInput() },
                { typeof(DEntitiesController), new DEntitiesController() },
                { typeof(DRenderingController), new DRenderingController() },
                { typeof(DPhysicsController), new DPhysicsController() },
                { typeof(DEditorSystem), new DEditorSystem() },
                
            };
        }

        public static T Get<T>() where T : EngineSystemBase
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            else
            {
                UnityEngine.Debug.LogError($"Cannot find service of type: {type}");
            }

            return default;
        }
    }
}
