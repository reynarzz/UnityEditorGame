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

        public Action<float, float, bool> OnHealthChanged { get; set; }
        public Action OnHealthDepleted { get; set; }

        public void SetInitialHealth(int initial)
        {
            _maxHealth = initial;
            currentHealth = _maxHealth;
        }

        public void AddAmount(float amount)
        {
            var increased = amount > 0;
            currentHealth += amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                OnHealthDepleted?.Invoke();
            }

            OnHealthChanged?.Invoke(currentHealth, _maxHealth, increased);

           
        }


    }
}
