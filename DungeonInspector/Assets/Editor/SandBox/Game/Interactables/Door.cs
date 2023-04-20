using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class Door : InteractableBase
    {
        private DRendererComponent _renderer;

        protected override void OnAwake()
        {
            _renderer = GetComp<DRendererComponent>();
        }

        public override void OnInteracted(InteractionType interaction, Actor actor)
        {

        }
    }
}
