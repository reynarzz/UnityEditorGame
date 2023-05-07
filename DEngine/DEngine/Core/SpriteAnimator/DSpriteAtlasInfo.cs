using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    [CreateAssetMenu]
    public class DSpriteAtlasInfo : ScriptableObject
    {
        [SerializeField] private DVec2 _spriteBlockSize = new DVec2(16, 16);
        [SerializeField] private Texture2D _texture;

        public DVec2 BlockSIze { get => _spriteBlockSize; set => _spriteBlockSize = value; }
        public Texture2D Texture { get => _texture; set => _texture = value; }
    }
}
