using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DComponent
    {
        
    }

    public abstract class DUpdatableComponent : DComponent
    {
        public virtual void Start() { }
        public virtual void Update() { }
    }
}
