using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class EnemyBase : Actor
    {
        private HealthBarUI _healthBar;
        private DBoxCollider _collider;
        private const float _healthBarYOffset = 0.3f;
        private ActorHealth _health;

        protected override void OnAwake()
        {
            Transform.Offset = new DVector2(0, 0.7f);
            _healthBar = GetComp<HealthBarUI>();
            _collider = GetComp<DBoxCollider>();
            _health = GetComp<ActorHealth>();

            _health.OnHealthDepleted += OnHealthDepleted;
        }

        protected override void OnUpdate()
        {
            _healthBar.Transform.Position = new DVector2(Transform.Position.x, _collider.AABB.Max.y + _healthBarYOffset);
        }

        protected virtual void OnHealthDepleted()
        {
            Entity.Destroy();
        }

        public override void OnDestroy()
        {
            _health.OnHealthDepleted -= OnHealthDepleted;
        }
    }
}