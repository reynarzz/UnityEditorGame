using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    [Serializable]
    public class DSpriteAnimation
    {
        [SerializeField] private DSpriteAtlas _atlas;
        private int _spriteIndex = 0;
        private float _time;
        private bool _play = false;

        public float Speed { get; set; } = 1;

        private Texture2D _currentTex;
        public Texture2D CurrentTexture => _currentTex;

        public DSpriteAnimation() { }
        public DSpriteAnimation(DSpriteAtlas spriteAtlas)
        {
            _atlas = spriteAtlas;

            if(_atlas != null)
            {
                _currentTex = _atlas.GetTexture(0);
            }
        }

        public void Update(float dt)
        {
            if (_play && _atlas != null)
            {
                if (_spriteIndex >= _atlas.TextureCount)
                {
                    _spriteIndex = 0;
                }

                _currentTex = _atlas.GetTexture(_spriteIndex);

                _time += dt * Speed;

                if (_time >= 1f)
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

        public string[] GetSpriteNames()
        {
            var names = default(string[]);

            if (_atlas != null)
            {
                names = new string[_atlas.TextureCount];

                for (int i = 0; i < _atlas.TextureCount; i++)
                {
                    names[i] = _atlas.GetTexture(i).name;
                }

            }

            return names;
        }
    }
}
