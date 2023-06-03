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
        public DSpriteAtlas Atlas { get; set; }
        private int _spriteIndex = 0;
        private float _time;
        private bool _play = false;

        public float Speed { get; set; } = 1;

        private Texture2D _currentTex;
        public Texture2D CurrentTexture => _currentTex;
        public bool Loop { get; set; } = true;
        public event Action<int> OnFrameStart;

        public DSpriteAnimation() { }
        public DSpriteAnimation(DSpriteAtlas spriteAtlas)
        {
            Atlas = spriteAtlas;

            if (Atlas != null)
            {
                _currentTex = Atlas.GetTexture(0);
            }
        }

        public void Update(float dt)
        {
            if (_play && Atlas != null)
            {
                if (_spriteIndex >= Atlas.TextureCount && Loop)
                {
                    _spriteIndex = 0;
                    OnFrameStart?.Invoke(_spriteIndex);
                }

                if (_spriteIndex < Atlas.TextureCount)
                {
                    _currentTex = Atlas.GetTexture(_spriteIndex);

                    _time += dt * Speed;

                    if (_time >= 1f)
                    {
                        _time = 0;

                        _spriteIndex++;

                        OnFrameStart?.Invoke(_spriteIndex);
                    }
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

            if (Atlas != null)
            {
                names = new string[Atlas.TextureCount];

                for (int i = 0; i < Atlas.TextureCount; i++)
                {
                    names[i] = Atlas.GetTexture(i).name;
                }

            }

            return names;
        }
    }
}
