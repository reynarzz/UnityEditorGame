using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class SpikeFloorTB : TileBehaviorBase<SpikeFloorTD>
    {
        protected override void OnEnter(Actor actor, SpikeFloorTD data, DTile tile)
        {
            Debug.Log("enter");
            Check(actor, tile);
        }

        protected override void OnUpdate(Actor actor, SpikeFloorTD data, DTile tile)
        {
            Check(actor, tile);
        }

        private void Check(Actor actor, DTile tile)
        {
            if (tile.Animation.CurrentFrame >= 2)
            {
                var health = actor.GetComp<ActorHealth>();

                if (health != null)
                {
                    health.InflictDamage(2);
                }
            }
        }
        protected override void OnExit(Actor actor, SpikeFloorTD data, DTile tile)
        {
            Debug.Log("exit");
        }
    }
}
