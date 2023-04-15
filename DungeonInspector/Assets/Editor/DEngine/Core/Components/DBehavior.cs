using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DBehavior : DTransformableComponent
    {
        public DGameEntity GameEntity { get; set; }

        public virtual void OnDestroy() { }
        public virtual void OnStart() { }
        public virtual void UpdateFrame() { }

        public T GetComponent<T>() where T : DComponent, new()
        {
            return GameEntity.GetComponent<T>();
        }

        public T AddComponent<T>() where T : DComponent, new()
        {
            return GameEntity.AddComponent<T>();
        }

        public DGameEntity FindGameEntity(string name)
        {
            return DIEngineCoreServices.Get<DEntitiesController>().FindGameEntity(name);
        }
    }
}
