using PathfindingAi.Astar;

namespace ConsoleInterface
{
    public class FactsHeuristicCalculator : ICalculateHeuristicCost
    {
        public float GetHeuristicCostBetween(IAstarPathNode fromNode, IAstarPathNode toNode)
        {
            var fromState = (AstarGoapState)fromNode;
            var toState = (AstarGoapState)toNode;

            return toState.DistanceFrom(fromState);
        }
    }
}