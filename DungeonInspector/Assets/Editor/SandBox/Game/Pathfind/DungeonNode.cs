using AStar.Collections.MultiDimensional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonPathNode : IBaseNode
    {
        public DTile Tile { get; set; }
        public DVector2 PosTest { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsOpen => !IsOccupied && Tile.IsWalkable;
    }
}
