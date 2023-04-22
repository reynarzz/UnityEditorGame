using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AStar;
using UnityEngine;

namespace DungeonInspector
{
    public class NavWorld
    {
        private readonly DTilemap _tileMap;

        private List<Position> _path;
        private DAABB _bounds;
        private int _width;
        private int _heigth;
        public NavWorld(DTilemap tilemap)
        {
            _tileMap = tilemap;

        }

        public void Init()
        {
            _bounds = _tileMap.GetTilemapBoundaries();

            _width = (int)MathF.Round(MathF.Abs(_bounds.Max.x - _bounds.Min.x));
            _heigth = (int)MathF.Round(MathF.Abs(_bounds.Max.y - _bounds.Min.y));

            var world = new DungeonGrid(_heigth, _width);

            var pathfind = new PathFinder<DungeonGrid>(world, new AStar.Options.PathFinderOptions() { UseDiagonals = false, HeuristicFormula = AStar.Heuristics.HeuristicFormula.Euclidean });


            for (int j = 0; j < _heigth; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    var x = _bounds.Min.x + i;
                    var y = _bounds.Min.y + j;

                    var tile = _tileMap.GetTile(x, y, 0);

                    if (tile != null)
                    {
                        world[new DVector2(i, j)] = new DungeonPathNode() { PosTest = new DVector2(x, y), Tile = tile };
                    }
                    //else
                    //{
                    //    world[new DVector2(i, j)] = null;
                    //}
                }
            }

            _path = pathfind.FindPath(WorldPosToGrid(new DVector2(0, 0)), WorldPosToGrid(new DVector2(7, -1))).ToList();
        }

        private DVector2 WorldPosToGrid(DVector2 pos)
        {
            return new DVector2(pos.x, pos.y) - new DVector2(_bounds.Min.x, _bounds.Min.y);
        }

        public void Update()
        {
            for (int i = 0; i < _path.Count; i++)
            {
                Utils.DrawSquare(_path[i] + new DVector2(_bounds.Min.x, _bounds.Min.y), DVector2.One * 0.2f);
            }
        }
    }
}