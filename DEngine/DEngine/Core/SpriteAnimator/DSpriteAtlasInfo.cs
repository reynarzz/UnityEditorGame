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
        [SerializeField] private int _spriteBlockSize = 16;
        [SerializeField] private Texture2D _texture;
        public int BlockSIze => _spriteBlockSize;
        public Texture2D Texture => _texture;
    }
}
