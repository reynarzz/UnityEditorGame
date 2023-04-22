using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AStar.Collections.MultiDimensional;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;
using DungeonInspector;
using UnityEditor.Graphs;

namespace AStar
{
    public class PathFinder<T> : IFindAPath<DVector2> where T : Grid<IBaseNode> 
    {
        //private const int ClosedValue = 0;
        private const int DistanceBetweenNodes = 1;
        private readonly PathFinderOptions _options;
        private readonly T _world;
        private readonly ICalculateHeuristic _heuristic;

        private Func<DVector2, DVector2> _coordConvert;
        private PathFinderGraph _graph;

        public PathFinder(T worldGrid, PathFinderOptions pathFinderOptions = null, Func<DVector2, DVector2> gridCoordConverted = null)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
            _heuristic = HeuristicFactory.Create(_options.HeuristicFormula);

            _coordConvert = gridCoordConverted;

            _graph = new PathFinderGraph(_world.Height, _world.Width, _options.UseDiagonals);
        }

        ///<inheritdoc/>
        public List<DVector2> FindPath(Position start, Position end)
        {
            var nodesVisited = 0;
            //--_graph = new PathFinderGraph(_world.Height, _world.Width, _options.UseDiagonals);
            _graph.Initialise();

            var startNode = new PathFinderNode(position: start, g: 0, h: 2, parentNodePosition: start);
            _graph.OpenNode(startNode);

            while (_graph.HasOpenNodes)
            {
                var q = _graph.GetOpenNodeWithSmallestF();
                
                if (q.Position == end)
                {
                    return OrderClosedNodesAsArray(_graph, q);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return null;
                }

                foreach (var successor in _graph.GetSuccessors(q))
                {
                    if (!_world[successor.Position]?.IsOpen ?? true)
                    {
                        continue;
                    }

                    var newG = q.G + DistanceBetweenNodes;

                    if (_options.PunishChangeDirection)
                    {
                        var qIsHorizontallyAdjacent = q.Position.Row - q.ParentNodePosition.Row == 0;
                        var successorIsHorizontallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;
                        
                        if (successorIsHorizontallyAdjacentToQ)
                        {
                            if (qIsHorizontallyAdjacent)
                            {
                                newG += Math.Abs(successor.Position.Row - end.Row) + Math.Abs(successor.Position.Column - end.Column);
                            }
                        }

                        var successorIsVerticallyAdjacentToQ = successor.Position.Column - q.Position.Column != 0;
                        if (successorIsVerticallyAdjacentToQ)
                        {
                            if (!qIsHorizontallyAdjacent)
                            {
                                newG += Math.Abs(successor.Position.Row - end.Row) + Math.Abs(successor.Position.Column - end.Column);
                            }
                        }
                    }

                    var updatedSuccessor = new PathFinderNode(
                        position: successor.Position,
                        g: newG,
                        h:_heuristic.Calculate(successor.Position, end),
                        parentNodePosition: q.Position);
                    
                    if (BetterPathToSuccessorFound(updatedSuccessor, successor))
                    {
                        _graph.OpenNode(updatedSuccessor);
                    }
                }

                nodesVisited++;
            }

            return null;
        }


        public void ReleasePath()
        {

        }

        private bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
        {
            return !currentSuccessor.HasBeenVisited ||
                (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);
        }

        //private List<DVector2> path;

        private List<DVector2> OrderClosedNodesAsArray(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
        {
            //  path.Clear();
            List<DVector2> path = new List<DVector2>();

            var currentNode = endNode;

            while (currentNode.Position != currentNode.ParentNodePosition)
            {
                Position pos = currentNode.Position;

                if (_coordConvert != null)
                {
                    pos = _coordConvert(pos);
                }

                path.Insert(0, pos);

                currentNode = graph.GetParent(currentNode);
            }
            
            {
                Position pos = currentNode.Position;

                if (_coordConvert != null)
                {
                    pos = _coordConvert(pos);
                }

                path.Insert(0, pos);
            }
            
            
            return path;
        }
    }
}