using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using RPG_PigeonAstronaute.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_PigeonAstronaute.Controls
{
    internal class PathFinding
    {
        private readonly List<PathNode> openList, closedList;
        private const int MaxDepth = 500;
        private Mouvement mvt = new Mouvement();


        public PathFinding()
        {
            openList = new List<PathNode>();
            closedList = new List<PathNode>();
        }

        public IList<Vector2> FindPath(Vector2 startTilePos, Vector2 goalTilePos, IList<TiledMapTile> collisions)
        {
            if (startTilePos == goalTilePos)
                return new List<Vector2>();
            openList.Clear();
            closedList.Clear();
            openList.Add(new PathNode(startTilePos, goalTilePos, null));
            CheckForPath(openList.First(), goalTilePos, collisions, 0);
            var node = closedList.Last();
            var path = new List<Vector2>();
            while(node.Parent != null)
            {
                path.Add(node.TilePosition);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }

        private void CheckForPath(PathNode currentNode, Vector2 goalTilePos, IList<TiledMapTile> collisions, int depth)
        { 
            if (depth == MaxDepth)
                return;
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            if (currentNode.TilePosition == goalTilePos)
                return;
            CheckNode(Directions.Up, currentNode, goalTilePos, collisions);
            CheckNode(Directions.Right, currentNode, goalTilePos, collisions);
            CheckNode(Directions.Down, currentNode, goalTilePos, collisions);
            CheckNode(Directions.Left, currentNode, goalTilePos, collisions);
            openList.Sort(new PathNodeCompoarer());
            if (openList.Any())
            {
                CheckForPath(openList.First(), goalTilePos, collisions, ++depth);
            }
        }

        private void CheckNode(Directions direction, PathNode currentNode, Vector2 goalTilePos, IList<TiledMapTile> collisions)
        {
            Vector2 dirVector = mvt.ConvertDirectionToVector(direction);
            Vector2 newPos = currentNode.TilePosition + dirVector;
            if(!collisions.Any(c => c.GlobalIdentifier != 0))
            {
                var oldNode = openList.FirstOrDefault(o => o.TilePosition == newPos);
                if (oldNode != null)
                    if (currentNode.CostFromStartPosition + PathNode.TileCost < oldNode.CostFromStartPosition)
                    {
                        oldNode.Parent = currentNode;
                        oldNode.CostFromStartPosition = currentNode.CostFromStartPosition + PathNode.TileCost;
                    }
                    else openList.Add(new PathNode(newPos, goalTilePos, currentNode));
            }
        }
    }
}
