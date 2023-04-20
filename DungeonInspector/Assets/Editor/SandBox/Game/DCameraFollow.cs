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
        private Perlin _perlin;
        public float Speed { get; set; } = 0;
        protected override void OnStart()
        {
            _camera = GetComp<DCamera>();
            _player = DGameEntity.FindGameEntity("Player").GetComp<Player>();
            _perlin = new Perlin();
        }

        protected override void OnUpdate()
        {
            var playerPos = new DVector2(_player.Transform.Position.x, _player.Transform.Position.y);

            var speed = Speed;
            var amplitude = 1.5f;
            var noiseX = (Mathf.PerlinNoise(Mathf.Cos(DTime.Time * speed) * 2 - 1, Mathf.Cos(DTime.Time * speed) * 2 - 1) - 0.5f) * 2;

            var noiseY = (Mathf.PerlinNoise(Mathf.Sin(DTime.Time * speed) * 2 - 1, Mathf.Sin(DTime.Time * speed) * 2 - 1) - 0.5f) * 2;


            _camera.Transform.Position = UnityEngine.Vector2.Lerp(_camera.Transform.Position, playerPos + new DVector2(noiseX, noiseY) * amplitude, 7 * DTime.DeltaTime);
            //_camera.Transform.Position = playerPos;

            if(Speed > 0)
            {
                Speed -= DTime.DeltaTime;
                if (Speed < 0)
                    speed = 0;
            }
        }
    }
}
