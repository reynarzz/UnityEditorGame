using AStar;
using AStar.Collections.MultiDimensional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonGrid : Grid<IBaseNode>
    {
        public DungeonGrid(int height, int width) : base(height, width) {  }
    }
}
