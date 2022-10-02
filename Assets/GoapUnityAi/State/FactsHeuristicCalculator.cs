using PathfindingAi.Astar;

namespace GoapUnityAi
{
    public class FactsHeuristicCalculator : ICalculateHeuristicCost
    {
        public float GetHeuristicCostBetween(IAstarPathNode fromNode, IAstarPathNode toNode)
        {
            var fromState = (AgentState) fromNode;
            var toState = (AgentState)toNode;

            return toState.DistanceFrom(fromState);
        }
    }
}
