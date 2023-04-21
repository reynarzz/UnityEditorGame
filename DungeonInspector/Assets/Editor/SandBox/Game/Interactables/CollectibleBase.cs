using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class CollectibleBase : DBehavior
    {
        protected virtual string SoundToPlay { get; set; }

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            var actor = collider.GetComp<Actor>();

            if (actor != null)
            {
                OnCollected(actor);

                Entity.Destroy();
            }
        }

        public abstract void OnCollected(Actor actor);
    }
}
