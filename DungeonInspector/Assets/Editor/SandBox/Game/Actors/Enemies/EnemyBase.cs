using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class EnemyBase : Actor
    {
        private HealthBarUI _healthBar;
        private DBoxCollider _collider;
        private const float _healthBarYOffset = 0.3f;
        private ActorHealth _health;
        private DRendererComponent _renderer;

        private const float _isHitMaxTime = 0.23f;
        private float _isHitTime;
        private bool _isHit;

        protected override void OnAwake()
        {
            Transform.Offset = new DVector2(0, 0.7f);
            _healthBar = GetComp<HealthBarUI>();
            _collider = GetComp<DBoxCollider>();
            _health = GetComp<ActorHealth>();
            _renderer = GetComp<DRendererComponent>();


            _health.OnHealthDepleted += OnHealthDepleted;
            _health.OnHealthDecreased += OnHealthDecreased;
        }

        private void OnHealthDecreased(float amount)
        {
            _renderer.SetMatInt("_isHit", 1);
            _isHitTime = 0;
            _isHit = true;
        }

        protected override void OnUpdate()
        {
            _healthBar.Transform.Position = new DVector2(Transform.Position.x, _collider.AABB.Max.y + _healthBarYOffset);

            if (_isHit)
            {
                _isHitTime += DTime.DeltaTime;

                if (_isHitTime >= _isHitMaxTime)
                {
                    _renderer.RemoveMatValue("_isHit");
                    _isHit = false;
                    _isHitTime = 0;
                }
            }
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