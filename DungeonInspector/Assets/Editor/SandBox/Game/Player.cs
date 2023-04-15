using System;
using UnityEngine;

namespace DungeonInspector
{
    public class Player : DBehavior
    {
        public override void Start()
        {

        }

        public override void Update()
        {
            var e = Event.current; 
            if (e.keyCode == KeyCode.F)
            {
                GameEntity.Destroy();
            }
            Transform.Position = new DVector2(MathF.Cos(DTime.TimeSinceStarted), MathF.Sin(DTime.TimeSinceStarted));
        }
    }
}