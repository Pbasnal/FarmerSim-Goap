using System.Collections.Generic;
using System.Linq;
using GoapAi;
using InsightsLogger;
using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;

namespace GoapAstarAi
{
    public class AstarAlgorithmWrapper<A, T> : IFindActionPlan<A, T>
        where A : IGoapAction<A, T>, IWeightedEdge
        where T : IAstarGoapNode<T>
    {
        private readonly AstarAlgorithm pathFinder;
        private readonly ISimpleLogger logger;

        public AstarAlgorithmWrapper(
            AstarGraph astarGraph,
            ICalculateHeuristicCost factsHeuristics,
            ISimpleLogger logger)
        {
            this.logger = logger;
            pathFinder = new AstarAlgorithm(astarGraph, factsHeuristics, logger);
        }

        public IList<A> FindPlanForAgent(IUseGoapPlan<A, T> agent)
        {
            var goalStateOfAgent = agent.GetGoalStateToPlanFor();
            var currentStateOfAgent = agent.GetCurrentState();
            if (goalStateOfAgent == null || currentStateOfAgent == null)
            {
                return null;
            }

            IAstarPathNode currentState = (IAstarPathNode)agent.GetCurrentState();

            var edges = new List<IWeightedEdge>();

            foreach (var action in agent.GetAgentActions())
            {
                edges.Add((IWeightedEdge)action);
            }

            var path = pathFinder.FindPathBetweenStates(
                currentState,
                goalStateOfAgent,
                edges);

            return path.Select(s => (A)s)
                    .ToList();
        }
    }
}