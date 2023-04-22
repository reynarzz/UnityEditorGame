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
        private DRendererComponent _renderer;
        private HealthBarUI _healthBar;
        private DBoxCollider _collider;
        protected ActorHealth _health;

        private const float _healthBarYOffset = 0.3f;
        private const float _isHitMaxTime = 0.25f;

        private float _isHitTime;
        private bool _isHit;

        public string Tag { get; set; } = "Player";
        public Actor Target { get; set; }

        protected override void OnAwake()
        {
            Transform.Offset = new DVector2(0, 0.7f);
            _healthBar = GetComp<HealthBarUI>();
            _collider = GetComp<DBoxCollider>();
            _health = GetComp<ActorHealth>();
            _renderer = GetComp<DRendererComponent>();

            _health.OnHealthChanged += OnHealthChanged;
            _health.OnHealthDepleted += OnHealthDepleted;
        }

        private void OnHealthChanged(float amount, float max, bool increased)
        {
            if (!increased)
            {
                _renderer.SetMatInt("_isHit", 1);
                _isHitTime = 0;
                _isHit = true;
            }

            _healthBar.OnChancePercentage(amount / max);
        }

        //protected override void OnTriggerEnter(DBoxCollider collider)
        //{
        //    if (collider.Entity.Tag == Tag)
        //    {
        //        _health.AddAmount(-1);
        //    }
        //}

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
            _health.OnHealthChanged -= OnHealthChanged;
        }
    }
}