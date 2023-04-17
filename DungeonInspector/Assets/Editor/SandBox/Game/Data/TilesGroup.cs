using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    [CreateAssetMenu]
    public class TilesGroup : ScriptableObject
    {
        [SerializeField] private TileData[] _tiles;

        public int TextureCount => _tiles.Length;

        public TileData GetTile(int index)
        {
            return _tiles[index];
        }
    }
}
