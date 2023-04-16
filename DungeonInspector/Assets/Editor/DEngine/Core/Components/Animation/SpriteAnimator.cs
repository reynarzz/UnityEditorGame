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

        private int _currentAnimIndex;

        public float Speed { get; set; } = -1;

        private DRendererComponent _renderer;

        public override void OnStart()
        {
            _animations = new List<SpriteAnimation>();
            _renderer = GetComp<DRendererComponent>();
        }
          
        public override void OnUpdate()
        {
            if(_animations.Count > 0)
            {
                _animations[_currentAnimIndex].Update(DTime.DeltaTime);
            }
        }

        public void Play(int index)
        {
            if (_currentAnimIndex != index)
            {
                // Reset previous
                _animations[_currentAnimIndex].Stop();

                _currentAnimIndex = index;
                
                if(Speed >= 0)
                {
                    _animations[_currentAnimIndex].Speed = Speed;
                }
                _animations[_currentAnimIndex].Play();
            }

            _renderer.Texture = _animations[_currentAnimIndex].CurrentTexture;
        }

        public void AddAnimation(params SpriteAnimation[] animation)
        {
            for (int i = 0; i < animation.Length; i++)
            {
                _animations.Add(animation[i]);
            }
        }


        public void Stop()
        {
            _animations[_currentAnimIndex].Stop();
        }

        public void Pause()
        {
            _animations[_currentAnimIndex].Pause();
        }
    }
}
