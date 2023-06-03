using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class ActorHealth : DBehavior
    {
        private float _maxHealth = 0;
        public float currentHealth { get; set; }

        public float NormalizedHealth => currentHealth / _maxHealth;

        public Action<float, float, bool> OnHealthChanged { get; set; }
        public Action OnHealthDepleted { get; set; }
        public Action<float> OnHealthSet { get; set; }
        public Action<float> OnDamage { get; set; }

        public float DamageColdown { get; set; } = 0;
        private float _currentDamageCouldownValue = 0;

        public void SetInitialHealth(int initial)
        {
            _maxHealth = initial;
            currentHealth = _maxHealth;
            OnHealthSet?.Invoke(currentHealth);
        }

        public void AddAmount(float amount)
        {
            var increased = amount > 0;
            currentHealth += amount;
           
            OnHealthChanged?.Invoke(currentHealth, _maxHealth, increased);

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                OnHealthDepleted?.Invoke();
            }
        }

        protected override void OnUpdate()
        {
            if(_currentDamageCouldownValue > 0)
            {
                _currentDamageCouldownValue -= DTime.DeltaTime;
            }
        }

        public void InflictDamage(float amount)
        {
            if(_currentDamageCouldownValue <= 0)
            {
                _currentDamageCouldownValue = DamageColdown;

                AddAmount(-amount);

                OnDamage?.Invoke(amount);
            }
        }
    }
}