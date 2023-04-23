using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class PlayerHealthUI : DBehavior
    {
        private bool _started = false;

        private HeartUI[] _hearts;

        [ExposeSlider(0, 1)] private float _healthTest;

        protected override void OnAwake()
        {

        }

        protected override void OnUpdate()
        {
            if (!_started)
            {
                var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

                _started = true;

                _hearts = new HeartUI[]
                {
                    gameMaster.PrefabInstantiator.InstanceHeartUI("heart1"),
                    gameMaster.PrefabInstantiator.InstanceHeartUI("heart2"),
                    gameMaster.PrefabInstantiator.InstanceHeartUI("heart3")
                };

                for (int i = 0; i < _hearts.Length; i++)
                {
                    _hearts[i].Transform.Position = new DVec2(i + 9, 6f);
                }
            }

            var index = _healthTest * 2 / _hearts.Length;

            var fract =  (int)((index - (int)index) * _hearts.Length);

            Debug.Log(fract);

            _hearts[(int)index].SetSpriteIndex(fract);

        }
    }
}
