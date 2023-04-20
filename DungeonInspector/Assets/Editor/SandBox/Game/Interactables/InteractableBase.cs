using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public enum InteractionType
    {
        Button,
        Collision
    }

    public abstract class InteractableBase : DBehavior
    {
        public virtual void OnInteracted(InteractionType interaction, Actor actor)
        {

        }
    }
}
