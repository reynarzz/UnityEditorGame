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

        public string Name => GameEntity.Name;
        public virtual void OnDestroy() { }
        public virtual void OnAwake() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }

        public T GetComp<T>() where T : DComponent, new()
        {
            return GameEntity.GetComp<T>();
        }

        public T AddComp<T>() where T : DComponent, new()
        {
            return GameEntity.AddComponent<T>();
        }

        public DGameEntity FindGameEntity(string name)
        {
            return DIEngineCoreServices.Get<DEntitiesController>().FindGameEntity(name);
        }
    }
}
