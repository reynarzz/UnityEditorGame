using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class ActorHealth : DBehavior
    {
        private const float _maxHealth = 10;

        public float Health { get; set; } = _maxHealth;
        public string EnemyTag { get; set; }

        private HealthBarUI _healthBar;
        public Action OnHealthDepleted { get; set; }

        protected override void OnAwake()
        {
            _healthBar = GetComp<HealthBarUI>();

            if (_healthBar != null)
            {
                _healthBar.Percentage = (Health / _maxHealth);
            }
        }

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            if (_healthBar != null && collider.Entity.Tag == EnemyTag)
            {
                Health--;

                _healthBar.OnChancePercentage(Health / _maxHealth);

                if(Health == 0)
                {
                    OnHealthDepleted?.Invoke();
                }
            }
        }
    }
}
