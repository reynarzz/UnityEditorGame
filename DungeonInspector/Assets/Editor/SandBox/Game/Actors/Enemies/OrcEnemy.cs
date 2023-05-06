using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AStar;

namespace DungeonInspector
{
    public class OrcEnemy : EnemyBase
    {
        private Player _player;
        private DRendererComponent _renderer;
        protected override int StartingHealth => 30;

        protected override void OnAwake()
        {
            base.OnAwake();

            _health.SetInitialHealth(5);
        }

        protected override void OnStart()
        {
            base.OnStart();

            _player = DGameEntity.FindGameEntity("Player").GetComp<Player>();
            _renderer = GetComp<DRendererComponent>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            //var sign = Math.Sign(_player.Transform.Position.x - Transform.Position.x);
            //if (sign != 0)
            //    _renderer.FlipX = -1 == sign;
        }
    }
}
