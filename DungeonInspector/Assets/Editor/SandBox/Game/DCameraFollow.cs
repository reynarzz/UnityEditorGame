using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DCameraFollow : DBehavior
    {
        private DCamera _camera;
        private Player _player;

        protected override void OnStart()
        {
            _camera = GetComp<DCamera>();
            _player = GameEntity.FindGameEntity("Player").GetComp<Player>();
        }

        protected override void OnUpdate()
        {
            var playerPos = new DVector2(_player.Transform.Position.x, _player.Transform.Position.y);

            _camera.Transform.Position = UnityEngine.Vector2.Lerp(_camera.Transform.Position, playerPos, 7 * DTime.DeltaTime);
        }
    }
} 
