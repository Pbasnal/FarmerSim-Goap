using GoapAi;
using GoapAstarAi;
using InsightsLogger;
using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;

namespace ConsoleInterface
{
    public class GoapPlannerFactory<A, T>
        where T : IAstarGoapNode<T>
        where A : IGoapAction<A, T>, IWeightedEdge
    {
        public static GoapPlanner<A, T> GetAGoapPlanner(ISimpleLogger logger)
        {
            var frontier = new SortedSetFrontier(logger);
            var astarGraph = new AstarGraph(frontier, logger);

            var factsHeuristics = new FactsHeuristicCalculator();
            var pathFinder = new AstarAlgorithmWrapper<A, T>(astarGraph, factsHeuristics, logger);
            return new GoapPlanner<A, T>(pathFinder, logger);
        }
    }
}