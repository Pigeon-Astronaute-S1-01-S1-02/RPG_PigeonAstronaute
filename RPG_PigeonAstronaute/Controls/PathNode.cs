using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_PigeonAstronaute.Controls
{
    internal class PathNode
    {
        public const int TileCost = 10;
        public PathNode Parent { get; set; }
        public float CostFromStartPosition { get; set; }
        public float CostToGoalPosition { get; set; }
        public float TotalCostOfNode => CostFromStartPosition * CostToGoalPosition;
        public Vector2 TilePosition { get; set; }
        private Vector2 distance { get; set; }

        public PathNode(Vector2 tilePos, Vector2 goalTilePos, PathNode parent)
        {
            TilePosition = tilePos;
            Parent = parent;
            distance = new Vector2(Math.Abs(tilePos.X-goalTilePos.X), Math.Abs(tilePos.Y-goalTilePos.Y));
            CostToGoalPosition = (distance.X + distance.Y) * TileCost;
            CostFromStartPosition = TileCost;

            if (parent != null)
                CostFromStartPosition += parent.CostFromStartPosition;
        }
    }
}
