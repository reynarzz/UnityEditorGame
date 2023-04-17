using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class EnemyBase : Actor
    {
        protected override void OnAwake()
        {
            Transform.Offset = new DVector2(0, 0.7f);
        }
    }
}