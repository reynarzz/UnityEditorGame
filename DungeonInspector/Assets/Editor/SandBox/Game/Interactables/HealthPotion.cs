using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class HealthPotion : CollectibleBase
    {
        public override void OnCollected(Actor actor)
        {
            var health = actor.GetComp<ActorHealth>();

            if(health != null)
            {
                health.AddAmount(1);
            }

            DAudio.PlayAudio("PotionTaken");
        }
    }
}
