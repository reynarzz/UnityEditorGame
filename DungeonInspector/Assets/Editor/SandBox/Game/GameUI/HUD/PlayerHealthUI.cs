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
        private HeartUI[] _hearts;

        private ActorHealth _health;

        protected override void OnAwake()
        {
             _health = DGameEntity.FindGameEntity("Player").GetComp<ActorHealth>();

            _health.OnHealthSet += OnHealthSet;
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

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

        private void OnHealthSet(float obj)
        {
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].SetSpriteIndex(2);
            }
        }

        protected override void OnUpdate()
        {
            var index = _health.NormalizedHealth * _hearts.Length;

            var fract = (int)((index - (int)index) * _hearts.Length);

            if (index < _hearts.Length)
            {
                _hearts[(int)index].SetSpriteIndex(fract);
            }
        }

    }
}