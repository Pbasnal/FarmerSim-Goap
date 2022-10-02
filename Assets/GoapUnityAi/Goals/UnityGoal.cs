using System;
using GoapAi;

namespace GoapUnityAi
{
    [Serializable]
    public class UnityGoal : IGoapGoal<AgentState>
    {
        public int priority;

        public string goalName;
        public string GoalName => goalName;

        public AgentState preConditions;
        public AgentState PreConditions => preConditions;

        public AgentState goalState;
        public AgentState GoalState => goalState;

        public bool ShouldPursueGoapGoal(AgentState currentState) => PreConditions.IsSameAs(currentState);
    }
}