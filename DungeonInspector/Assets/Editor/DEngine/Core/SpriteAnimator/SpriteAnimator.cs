using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class SpriteAnimator
    {
        private readonly SpriteAnimation[] _animations;

        private int _currentAnimIndex;
        private Texture2D _tex;

        public Texture2D CurrentTex => _tex;
        public float Speed { get; set; } = -1;
        public SpriteAnimator(params SpriteAnimation[] animations)
        {
            _animations = animations;
        }

        public void Update(float dt)
        {
            _animations[_currentAnimIndex].Update(dt);
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

            _tex = _animations[_currentAnimIndex].CurrentTexture;
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
