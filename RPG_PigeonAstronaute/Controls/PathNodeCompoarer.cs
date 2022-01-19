using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_PigeonAstronaute.Controls
{
    internal class PathNodeCompoarer : IComparer<PathNode>
    {
        public int Compare(PathNode x, PathNode y)
        {
            return x.TotalCostOfNode == y.TotalCostOfNode 
                ? x.CostToGoalPosition.CompareTo(y.CostToGoalPosition) 
                : x.TotalCostOfNode.CompareTo(y.TotalCostOfNode);
        }
    }
}
