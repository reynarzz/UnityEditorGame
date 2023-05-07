using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DCameraFollow : DBehavior
    {
        private DCamera _camera;
        private Player _player;
        [DExpose]private float _speed = 7;
        protected override void OnStart()
        {
            _camera = GetComp<DCamera>();
            _player = DGameEntity.FindGameEntity("Player").GetComp<Player>();
        }
         
        protected override void OnUpdate()
        {
            var playerPos = new DVec2(_player.Transform.Position.x, _player.Transform.Position.y);

            var offset = (DInput.GetMouseWorldPos() - playerPos) * 0.09f;

            _camera.Transform.Position = UnityEngine.Vector2.Lerp(_camera.Transform.Position, playerPos + offset, _speed * DTime.DeltaTime);
            //_camera.Transform.Position = playerPos;
        }
    }
}
