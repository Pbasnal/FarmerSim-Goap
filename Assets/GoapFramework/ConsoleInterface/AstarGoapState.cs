using System;
using System.Collections.Generic;
using System.Text;
using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;

namespace ConsoleInterface
{
    public class AstarGoapState : IAstarGoapNode<AstarGoapState>
    {
        public string StateName { get; }
        protected IDictionary<string, bool> facts;
        public IAstarPathNode PreviousNode { get; set; }
        public IWeightedEdge EdgeFromPreviousNode { get; set; }
        public float CostOfPathToNode { get; set; }
        public float HeuristicCost { get; set; }
        public bool IsVisited { get; set; }
        public string Id { get; set; }

        public AstarGoapState(string stateName)
        {
            this.StateName = stateName;
            facts = new Dictionary<string, bool>();
        }

        public static float GetTotalCostOfNode(IAstarPathNode node) => node.CostOfPathToNode + node.HeuristicCost;

        public int CompareTo(IAstarPathNode? other)
        {
            return GetTotalCostOfNode(this).CompareTo(GetTotalCostOfNode(other));
        }

        public void Reset()
        {
            facts.Clear();
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var fact in facts)
            {
                str.Append($"{fact.Key}: {fact.Value}\n");
            }

            return str.ToString();
        }

        public float DistanceFrom(AstarGoapState fromNode)
        {
            var distance = Math.Abs(facts.Count - fromNode.facts.Count);

            foreach (var fact in facts)
            {
                if (fromNode.GetFact(fact.Key) != fact.Value)
                {
                    distance++;
                }
            }

            return distance;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not AstarGoapState state)
            {
                return false;
            }

            return IsSameAs(state);
        }

        public IDictionary<string, bool> GetAllFacts()
        {
            return facts;
        }

        public bool IsSameAs(AstarGoapState currentState)
        {
            foreach (var fact in facts)
            {
                if (currentState.GetFact(fact.Key) != fact.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool GetFact(string factName)
        {
            if (facts.ContainsKey(factName))
            {
                return facts[factName];
            }
            return false;
        }

        public void SetFact(string factName, bool factValue)
        {
            if (facts.ContainsKey(factName))
            {
                facts[factName] = factValue;
            }
            else
            {
                facts.Add(factName, factValue);
            }
        }

        public void RemoveFact(string factName)
        {
            if (facts.ContainsKey(factName))
            {
                facts.Remove(factName);
            }
        }

        public AstarGoapState Clone()
        {
            var state = new AstarGoapState(StateName);

            foreach (var factEntry in facts)
            {
                state.SetFact(factEntry.Key, factEntry.Value);
            }

            return state;
        }
    }
}