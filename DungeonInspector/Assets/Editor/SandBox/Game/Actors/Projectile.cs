using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class Projectile : DBehavior
    {
        private DVec2 _dir;

        private const float _autoDestoyTime = 2;
        private float _timeToDestoy;

        public void Shoot(DVec2 dir)
        {
            _dir = dir;
        }

        protected override void OnUpdate()
        {
            _timeToDestoy += DTime.DeltaTime;

            if(_timeToDestoy >= _autoDestoyTime)
            {
                Entity.Destroy();
            }

            Transform.Position += _dir * DTime.DeltaTime * 15;
        }

        protected override void OnTriggerStay(DBoxCollider collider)
        {
            if(collider.Entity.Tag == "Enemy")
            {
                collider.GetComp<ActorHealth>().AddAmount(-1.5f);
                Entity.Destroy();
            }
        }
    }
}
