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

        private Dictionary<Actor, (List<DVec2>, Color)> _path;
        private DAABB _bounds;
        private int _width;
        private int _heigth;
        private PathFinder<DungeonGrid> _pathfind;
        private bool _needsFindAPath;

        private Dictionary<Actor, List<EnemyBase>> _actorsToFindPath; // target by requesters

        public NavWorld(DTilemap tilemap)
        {
            _tileMap = tilemap;
            _path = new Dictionary<Actor, (List<DVec2>, Color)>();

            //// Uncomment to show paths
            //--DIEngineCoreServices.Get<DRenderingController>().AddDebugGUI(DrawNodes);
        }

        public void Init()
        {
            _bounds = _tileMap.GetTilemapBoundaries();
            _path.Clear();

            _width = (int)MathF.Round(MathF.Abs(_bounds.Max.x - _bounds.Min.x));
            _heigth = (int)MathF.Round(MathF.Abs(_bounds.Max.y - _bounds.Min.y));

            var world = new DungeonGrid(_heigth, _width);

            _pathfind = new PathFinder<DungeonGrid>(world, new AStar.Options.PathFinderOptions()
            {
                UseDiagonals = false, 
                HeuristicFormula = AStar.Heuristics.HeuristicFormula.Euclidean
            }, GridPosToWorld);

            _actorsToFindPath = new Dictionary<Actor, List<EnemyBase>>();

            for (int j = 0; j < _heigth; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    var x = _bounds.Min.x + i;
                    var y = _bounds.Min.y + j;

                    var tile = _tileMap.GetTile(x, y);

                    if (tile != null)
                    {
                        world[new DVec2(i, j)] = new DungeonPathNode() { /*PosTest = new DVec2(x, y),*/ Tile = tile };
                    }
                    //else
                    //{
                    //    world[new DVec2(i, j)] = null;
                    //}
                }
            }
        }

        private List<DVec2> GetPathToTarget(Actor requester, Actor target)
        {
            var path = _pathfind.FindPath(WorldPosToGrid(requester.Transform.RoundPosition), WorldPosToGrid(target.Transform.RoundPosition));

            // -- Debug
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
            //----


            return path;
        }

        private DVec2 WorldPosToGrid(DVec2 pos)
        {
            return new DVec2(pos.x, pos.y) - new DVec2(_bounds.Min.x, _bounds.Min.y);
        }

        private DVec2 GridPosToWorld(DVec2 pos)
        {
            return new DVec2(pos.x, pos.y) + new DVec2(_bounds.Min.x, _bounds.Min.y);
        }

        public void DrawNodes()
        {
            if (_path != null)
            {
                foreach (var paths in _path.Values)
                {
                    var c = GUI.color;
                    GUI.color = paths.Item2;

                    foreach (var item in paths.Item1)
                    {
                        Utils.DrawSquare(item, DVec2.One * 0.2f);

                    }

                    GUI.color = c;
                }

            }
        }

        public void RequestPath(EnemyBase enemy, Actor target)
        {
            // returns a non taken path, ordering by enemy distance to the target.
            if (!_actorsToFindPath.ContainsKey(target))
            {
                _actorsToFindPath.Add(target, new List<EnemyBase>() { enemy });
            }
            else
            {
                _actorsToFindPath[target].Add(enemy);
            }

            _needsFindAPath = true;
        }

        public void OnLateUpdate()
        {
            if (_needsFindAPath)
            {
                _needsFindAPath = false;

                foreach (var path in _actorsToFindPath)
                {
                    var target = path.Key;
                    var requesters = _actorsToFindPath[target].OrderBy(x => (x.Transform.Position - target.Transform.Position).SqrMagnitude);

                    foreach (var requester in requesters)
                    {
                        //Debug.Log(requester.Name);
                        requester.OnNewPath(GetPathToTarget(requester, target));
                    }
                }

                _pathfind.ReleasePath();
                _actorsToFindPath.Clear();
            }
        }
    }
}