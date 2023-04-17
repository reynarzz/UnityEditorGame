using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class Default_TB : TileBehaviorBase
    {
        protected override void OnEnter()
        {
            //if (Actor.TryGetComp<ActorHealth>(out var health))
            //{
            //    _LOG.LOG($"Actor: '{Actor.Name}' Health is: " + health.Health);
            //}
            //else
            //{
            //    $"Actor '{Actor.Name}', Enter".LOG();
            //}
        }

        protected override void OnExit()
        {
            $"Actor '{Actor.GameEntity.Name}', Exit".LOG();
        }
    }
}