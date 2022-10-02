using System;
using GoapAi;
using PathfindingAi.AstarWrapper;
using PathfindingAi.GraphStructure;

namespace ConsoleInterface
{
    public class AstarGoapAction : IAstarGoapEdge<AstarGoapAction, AstarGoapState>
    {
        public string ActionName { get; }
        public INode OriginNode => PreConditions;

        public INode DestinationNode => Effects;

        public float Cost { get; set; }

        public AstarGoapState PreConditions { get; set; }

        public AstarGoapState Effects { get; set; }

        public AstarGoapAction(string actionName)
        {
            ActionName = actionName;
        }

        public bool IsDestinationNode(INode node)
        {
            AstarGoapState pathNode = (AstarGoapState)node;
            return CanReachStateWithAction(pathNode);
        }

        public bool CanReachStateWithAction(AstarGoapState resultingState) => Effects.IsSameAs(resultingState);
    }
}