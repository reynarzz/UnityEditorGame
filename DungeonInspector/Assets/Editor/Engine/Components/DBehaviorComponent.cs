using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DBehaviorComponent : DUpdatableComponent
    {
        public DGameEntity GameEntity { get; set; }
        public DTransformComponent Transform => GameEntity.Transform;

        public virtual void OnDestroy() { }
    }
}
