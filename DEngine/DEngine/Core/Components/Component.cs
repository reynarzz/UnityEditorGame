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
        public DGameEntity Entity { get; set; }

        public string GUID => Entity.GUID;
        public bool Enabled { get; set; } = true;

        public void Destroy()
        {
            _isAlive = false;

            OnRemoved(this);
            OnRemoved = null;

            OnDestroy();
        }

        public virtual void OnDestroy() { }
    }

    public class DTransformableComponent : DComponent
    {
        public virtual DTransformComponent Transform { get; set; }
       
    }
}
