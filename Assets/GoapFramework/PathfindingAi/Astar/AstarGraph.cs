using System.Collections.Generic;
using InsightsLogger;
using PathfindingAi.GraphStructure;

namespace PathfindingAi.Astar
{
    public class AstarGraph : Graph<IAstarPathNode, IWeightedEdge>
    {
        private readonly IFrontier frontier;

        // private IAstarPathNode currentNode;
        private ISimpleLogger logger;

        public AstarGraph(IFrontier frontier, ISimpleLogger logger) : base(logger)
        {
            this.frontier = frontier;
            this.logger = logger;
        }

        public void ResetGraph()
        {
            if (Origin != null)
            {
                Origin.HeuristicCost = 0;
                Origin.CostOfPathToNode = 0;
                Origin.PreviousNode = null;
                Origin.IsVisited = false;
            }
            if (Destination != null)
            {
                Destination.HeuristicCost = 0;
                Destination.CostOfPathToNode = 0;
                Destination.PreviousNode = null;
                Destination.IsVisited = false;
            }
            
            // Needed to reset all the costs for all the edges before removing them from the lists
            foreach (var edge in Edges)
            {
                var originNode = (IAstarPathNode)edge.OriginNode;
                originNode.HeuristicCost = 0;
                originNode.CostOfPathToNode = 0;
                originNode.PreviousNode = null;
                originNode.IsVisited = false;

                var destinationNode = (IAstarPathNode)edge.DestinationNode;
                destinationNode.HeuristicCost = 0;
                destinationNode.CostOfPathToNode = 0;
                destinationNode.PreviousNode = null;
                destinationNode.IsVisited = false;
            }

            Edges.Clear();
            frontier.Reset();
        }

        public bool HasFrontierNodes() => !frontier.IsEmpty();

        public void AddFrontierNode(IAstarPathNode node)
        {
            frontier.AddNodeToFrontier(node);
            // logger.LogDebug($"\tprevNode: {node.PreviousNode?.StateName} edge: {node.EdgeFromPreviousNode}");
        }

        public IAstarPathNode GetNextClosestNode()
        {
            return frontier.GetClosestNode();
        }

        public List<IWeightedEdge> GetCalculatedPathTo(IAstarPathNode originNode)
        {
            var path = new List<IWeightedEdge>();

            while (originNode != null && originNode.EdgeFromPreviousNode != null)
            {
                logger.LogDebug($"Inserting in path:[{originNode.EdgeFromPreviousNode}]");
                path.Insert(0, originNode.EdgeFromPreviousNode);
                // logger.LogDebug($"Path: {originNode.NodeName} prevEdge {originNode.PreviousNode}");
                originNode = originNode.PreviousNode;
            }

            return path;
        }
    }
}