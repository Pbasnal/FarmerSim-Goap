using PathfindingAi.AstarWrapper;
using PathfindingAi.GraphStructure;
using UnityEngine;

namespace GoapUnityAi
{
    public abstract class ActionBehaviour : MonoBehaviour, IAstarGoapEdge<ActionBehaviour, AgentState>
    {
        public float cost = 1;
        [SerializeField] protected AgentState preconditions;
        [SerializeField] protected AgentState effects;

        public float Cost => cost;

        public string ActionName { get; protected set; }
        public INode OriginNode => preconditions;
        public INode DestinationNode => effects;
        public AgentState PreConditions => preconditions;
        public AgentState Effects => effects;

        public bool IsDestinationNode(INode node)
        {
            AgentState pathNode = (AgentState)node;
            return CanReachStateWithAction(pathNode);
        }

        public void UpdateAgentStateWithEffects(AgentState agentState)
        {
            // foreach (var effect in Effects.GetAllFacts())
            // {
            //     agentState.SetFact(effect.Key, effect.Value);
            // }
        }

        public virtual bool CanReachStateWithAction(AgentState resultingState) => Effects.IsSameAs(resultingState);

        public abstract int ExecuteAction(GoapAgent agent);
    }
}
