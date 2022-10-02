using UnityEngine;
using Library.Logging;

namespace GoapUnityAi
{
    public class AgentStateBehaviour : MonoBehaviour
    {
        public UnityGoal goal;
        public AgentState state;

        private void Start() 
        {
            // DebugLogger.LogDebug($"Listing all facts");
            foreach (var fact in state.GetAllFacts())
            {
                // DebugLogger.LogDebug($"{fact.Key} : {fact.Value}");
            }

            // DebugLogger.LogDebug($"Listing goal {goal.GoalName} preconditions");
            foreach (var fact in goal.PreConditions.GetAllFacts())
            {
                // DebugLogger.LogDebug($"{fact.Key} : {fact.Value}");
            }

            // DebugLogger.LogDebug($"Listing goal {goal.GoalName} goal state");
            foreach (var fact in goal.GoalState.GetAllFacts())
            {
                // DebugLogger.LogDebug($"{fact.Key} : {fact.Value}");
            }
        }
    }
}