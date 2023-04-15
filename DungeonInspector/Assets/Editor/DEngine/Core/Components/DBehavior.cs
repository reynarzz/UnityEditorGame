using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DBehavior : DUpdatableComponent
    {
        public DGameEntity GameEntity { get; set; }
        public DTransformComponent Transform => GameEntity.Transform;

        public virtual void OnDestroy() { }

        public T GetComponent<T>() where T: DComponent, new()
        {
            return GameEntity.GetComponent<T>();
        }

        public T AddComponent<T>() where T : DComponent, new()
        {
            return GameEntity.AddComponent<T>();
        }
    }
}
