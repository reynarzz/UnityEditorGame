using AStar.Options;
using AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DungeonInspector
{
    public class PathFindingExample
    {
        public void Pathfinding()
        {
            var pathfinderOptions = new PathFinderOptions
            {
                PunishChangeDirection = true,
                UseDiagonals = false,
            };

            var tiles = new short[,]
            {
                { 1, 0, 1 },
                { 1, 0, 1 },
                { 1, 1, 1 },
            };
            // Everione has to use the same worldgrid instance to avoid collision.
            var worldGrid = new WorldGrid(tiles);
            var pathfinder = new PathFinder(worldGrid, pathfinderOptions);

            // The following are equivalent:

            // matrix indexing
            Position[] path = pathfinder.FindPath(new Position(0, 0), new Position(0, 2));

            // point indexing
            //Point[] path = pathfinder.FindPath(new Point(0, 0), new Point(2, 0));
        }
    }
}
