using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DComponent
    {
        public event Action<DComponent> OnRemoved;

        private bool _isAlive = true;
        public bool IsAlive => _isAlive;

        public void Destroy()
        {
            _isAlive = false;

            OnRemoved(this);
            OnRemoved = null;
        }
    }

    public class DUpdatableComponent : DComponent
    {
        public virtual void Start() { }
        public virtual void Update() { }
    }
}
