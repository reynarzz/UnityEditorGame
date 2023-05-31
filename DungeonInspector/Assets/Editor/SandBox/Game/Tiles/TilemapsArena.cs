using System;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class TilemapArena
    {
        private readonly DTilemap[] _tilemaps;

        public TilemapArena(DTilemap[] tilemaps)
        {
            _tilemaps = tilemaps;
        }
    }
}
