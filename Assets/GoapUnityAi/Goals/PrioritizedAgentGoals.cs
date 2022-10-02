using System.Collections.Generic;
using UnityEngine;
using Library.Logging;

namespace GoapUnityAi
{
    public class PrioritizedAgentGoals : MonoBehaviour
    {
        public List<UnityGoal> goals;

        private GoapAgent agent;

        private void Start()
        {
            goals.Sort(CompareGoalPriority);

            foreach (var goal in goals)
            {
                goal.preConditions.StateName = goal.GoalName + "-conditions";
                goal.goalState.StateName = goal.GoalName + "-goalState";
            }

            agent = GetComponent<GoapAgent>();
        }

        public UnityGoal GetGoal(AgentState currentState)
        {
            foreach (var unityGoal in goals)
            {
                if(unityGoal.ShouldPursueGoapGoal(currentState)) 
                {
                    // DebugLogger.LogInfo($"Agent can follow {unityGoal.GoalName} goal");
                    return unityGoal;
                }
            }
            return null;
        }

        private int CompareGoalPriority(UnityGoal goal1, UnityGoal goal2)
        {
            return goal1.priority.CompareTo(goal2.priority);
        }
    }
}