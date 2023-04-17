using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DAnimatorComponent : DBehavior
    {
        private List<SpriteAnimation> _animations;

        private int _currentAnimIndex = -1;

        public float Speed { get; set; } = -1;

        private DRendererComponent _renderer;

        protected override void OnAwake()
        {
            _animations = new List<SpriteAnimation>();
            _renderer = GetComp<DRendererComponent>();
        }


        protected override void OnUpdate()
        {
            if(_currentAnimIndex >= 0 && _animations.Count > 0)
            {
                _animations[_currentAnimIndex].Update(DTime.DeltaTime);
            }
        }

        public void Play(int index)
        {
            if (_currentAnimIndex != index && _animations.Count > index)
            {
                // Reset previous
                if(_currentAnimIndex >= 0)
                {
                    _animations[_currentAnimIndex].Stop();
                }

                _currentAnimIndex = index;
                
                if(Speed >= 0)
                {
                    _animations[_currentAnimIndex].Speed = Speed;
                }
                _animations[_currentAnimIndex].Play();
            }

            _renderer.Sprite = _animations[_currentAnimIndex].CurrentTexture;
        }

        public void AddAnimation(params SpriteAnimation[] animation)
        {
            for (int i = 0; i < animation.Length; i++)
            {
                _animations.Add(animation[i]);
            }

            Play(0);
        }


        public void Stop()
        {
            if(_currentAnimIndex >= 0)
            {
                _animations[_currentAnimIndex].Stop();
            }
            _currentAnimIndex = -1;
        }

        public void Pause()
        {
            if (_currentAnimIndex >= 0)
            {
                _animations[_currentAnimIndex].Pause();
            }
        }
    }
}
