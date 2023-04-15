using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace DungeonInspector
{
    public interface IDService {  }

    public class DIEngineCoreServices
    {
        private static Dictionary<Type, IDService> _services;

        public DIEngineCoreServices()
        {
            _services = new Dictionary<Type, IDService>()
            {
                { typeof(DEntitiesController), new DEntitiesController() },
                { typeof(DRenderingController), new DRenderingController() }
                
            };
        }

        public static T Get<T>() where T : IDService
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
