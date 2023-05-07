using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public class CameraShake : DBehavior
    {
        [DExpose] private float _freq = 15;

        private const float _amplitude = 15;
        private float _shakeTime = 0;

        [DExpose] private float _shakeDecreaseSpeed = 4;
        

        protected override void OnAwake()
        {
            
        }

        protected override void OnUpdate()
        {

            Transform.Position += (DVec2)new Vector2(
                Mathf.PerlinNoise(Mathf.Cos(DTime.Time * _freq * 0.66f), Mathf.Sin (DTime.Time * _freq * 0.66f)) * _amplitude, 
                Mathf.PerlinNoise(Mathf.Sin(DTime.Time * _freq), Mathf.Sin(DTime.Time * _freq)) * _amplitude) * DTime.DeltaTime * _shakeTime;

            _shakeTime -= _shakeDecreaseSpeed * DTime.DeltaTime;
            _shakeTime = Mathf.Clamp01(_shakeTime);

        }

        public void Shake()
        {
            _shakeTime = 1;
        }
    }
}
