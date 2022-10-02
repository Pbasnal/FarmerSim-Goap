using System;
using System.Collections.Generic;
using System.Linq;
using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;

namespace GoapUnityAi
{
    [Serializable]
    public class AgentState : IAstarGoapNode<AgentState>
    {
        public List<GoapFact> facts;

        public string StateName { get; set; }
        public IAstarPathNode PreviousNode { get; set; }
        public IWeightedEdge EdgeFromPreviousNode { get; set; }
        public float CostOfPathToNode { get; set; }
        public float HeuristicCost { get; set; }
        public bool IsVisited { get; set; }
        public string Id { get; set; }

        public IDictionary<GoapFactName, bool> GetAllFacts()
        {
            return facts.ToDictionary(f => f.fact, f => f.value);
        }

        public bool GetFact(GoapFactName factName)
        {
            var factIndex = facts.FindIndex(f => f.fact == factName);
            if (factIndex == -1)
            {
                return false;
            }

            return facts[factIndex].value;
        }

        public void SetFact(GoapFactName factName, bool factValue)
        {
            var indexOfFact = facts.FindIndex(f => f.fact == factName);
            if (indexOfFact != -1)
            {
                facts[indexOfFact].value = factValue;
            }
            else
            {
                facts.Add(new GoapFact()
                {
                    fact = factName,
                    value = factValue
                });
            }
        }

        public float DistanceFrom(AgentState fromNode)
        {
            var distance = Math.Abs(facts.Count - fromNode.facts.Count);

            foreach (var fact in facts)
            {
                if (fromNode.GetFact(fact.fact) != fact.value)
                {
                    distance++;
                }
            }

            return distance;
        }

        public bool IsSameAs(AgentState currentState)
        {
            foreach (var fact in facts)
            {
                if (currentState.GetFact(fact.fact) != fact.value)
                {
                    return false;
                }
            }

            return true;
        }

        public int CompareTo(IAstarPathNode other)
        {
            return GetTotalCostOfNode(this).CompareTo(GetTotalCostOfNode(other));
        }

        private static float GetTotalCostOfNode(IAstarPathNode node) => node.CostOfPathToNode + node.HeuristicCost;



        public override bool Equals(object obj)
        {
            if (obj is not AgentState state)
            {
                return false;
            }

            return IsSameAs(state);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(facts);
        }
    }
}

