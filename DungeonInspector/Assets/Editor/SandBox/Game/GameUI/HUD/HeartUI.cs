using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class HeartUI : DBehavior
    {
        private DSpriteAtlas _spriteAtlas;

        private DRendererComponent _renderer;

        public void Init(DSpriteAtlas atlas)
        {
            _spriteAtlas = atlas;
            _renderer = AddComp<DRendererComponent>();
            _renderer.TransformWithCamera = false;

            _renderer.Sprite = atlas.GetTexture(2);
        }

        public void SetSpriteIndex(int index)
        {
            if(index < _spriteAtlas.TextureCount)
            {
                _renderer.Sprite = _spriteAtlas.GetTexture(index);
            }
        }
    }
}