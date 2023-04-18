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
        [SerializeField] private DTile[] _tiles;

        public int Count => _tiles.Length;

        public DTile GetTile(int index)
        {
            var dTile = _tiles[index];
            dTile.AssetIndex = index;

            return dTile;
        }
    }
}
