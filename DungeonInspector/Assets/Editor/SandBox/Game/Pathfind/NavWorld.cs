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

        private Dictionary<Actor, (List<DVector2>, Color)> _path;
        private DAABB _bounds;
        private int _width;
        private int _heigth;
        private PathFinder<DungeonGrid> _pathfind;

        public NavWorld(DTilemap tilemap)
        {
            _tileMap = tilemap;
            _path = new Dictionary<Actor, (List<DVector2>, Color)>();
        }

        public void Init()
        {
            _bounds = _tileMap.GetTilemapBoundaries();

            _width = (int)MathF.Round(MathF.Abs(_bounds.Max.x - _bounds.Min.x));
            _heigth = (int)MathF.Round(MathF.Abs(_bounds.Max.y - _bounds.Min.y));

            var world = new DungeonGrid(_heigth, _width);

            _pathfind = new PathFinder<DungeonGrid>(world, new AStar.Options.PathFinderOptions() { UseDiagonals = false, HeuristicFormula = AStar.Heuristics.HeuristicFormula.Euclidean }, GridPosToWorld);


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
        }

        public List<DVector2> GetPathToTarget(Actor requester, Actor target)
        {
            var path = _pathfind.FindPath(WorldPosToGrid(requester.Transform.RoundPosition), WorldPosToGrid(target.Transform.RoundPosition));

            if (path != null)
            {
                var value = (path, new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1f));
                if (!_path.ContainsKey(requester))
                {
                    _path.Add(requester, value);

                }
                else
                {
                    _path[requester] = value;
                }
            }
            return path;
        }

        private DVector2 WorldPosToGrid(DVector2 pos)
        {
            return new DVector2(pos.x, pos.y) - new DVector2(_bounds.Min.x, _bounds.Min.y);
        }

        private DVector2 GridPosToWorld(DVector2 pos)
        {
            return new DVector2(pos.x, pos.y) + new DVector2(_bounds.Min.x, _bounds.Min.y);
        }

        public void Update()
        {
            if (_path != null)
            {
                foreach (var paths in _path.Values)
                {
                    var c = GUI.color;
                    GUI.color = paths.Item2;

                    foreach (var item in paths.Item1)
                    {
                        Utils.DrawSquare(item, DVector2.One * 0.2f);

                    }

                    GUI.color = c;
                }

            }

        }
    }
}