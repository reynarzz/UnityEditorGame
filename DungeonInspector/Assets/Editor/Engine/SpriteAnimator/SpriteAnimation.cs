using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class SpriteAnimation
    {
        private readonly E_SpriteAtlas _atlas;
        private int _spriteIndex = 0;
        private float _time;
        private bool _play = false;

        public float Speed { get; set; } = 1;
        private Texture2D _currentTex;
        public Texture2D CurrentTexture => _currentTex;
        public SpriteAnimation(E_SpriteAtlas spriteAtlas)
        {
            _atlas = spriteAtlas;
            _currentTex = _atlas.GetTexture(0);
        }

        public void Update(float dt)
        {
            if (_play)
            {
                if(_spriteIndex >= _atlas.TextureCount)
                {
                    _spriteIndex = 0;
                }

                _currentTex = _atlas.GetTexture(_spriteIndex);

                _time += dt * Speed;

                if(_time >= 1)
                {
                    _time = 0;

                    _spriteIndex++;
                }
            }
        }

        public void Play()
        {
            _play = true;
        }

        public void Stop()
        {
            _spriteIndex = 0;
            _time = 0;
            _play = false;
        }

        public void Pause()
        {
            _play = false;
        }
    }
}
