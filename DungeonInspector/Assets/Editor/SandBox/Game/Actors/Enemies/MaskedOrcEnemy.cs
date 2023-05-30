using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class MaskedOrcEnemy : EnemyBase
    {
        protected override int StartingHealth => 10;

        protected override void OnHealthChanged(float amount, float max, bool increased)
        {
            base.OnHealthChanged(amount, max, increased);

            DAudio.PlayAudio("OrcHit");
        }
    }
}
