using System.Collections.Generic;
using System.Linq;
using GoapAi;
using InsightsLogger;

namespace ConsoleInterface
{
    public class Agent : IUseGoapPlan<AstarGoapAction, AstarGoapState>
    {
        private readonly IDictionary<int, AstarGoapGoal> agentGoals;
        private readonly List<AstarGoapAction> agentActions;

        private readonly ISimpleLogger logger;

        private readonly AstarGoapState currentState;

        public Agent(ISimpleLogger logger)
        {
            this.logger = logger;


            currentState = new AstarGoapState("Starting State");
            agentGoals = new SortedDictionary<int, AstarGoapGoal>();
            agentActions = new List<AstarGoapAction>();
        }

        public AstarGoapState GetCurrentState() => currentState;
        public void AddAction(AstarGoapAction action) => agentActions.Add(action);
        public void RemoveAction(AstarGoapAction action) => agentActions.Add(action);

        public void AddGoal(int priority, AstarGoapGoal goal)
        {
            if (agentGoals.ContainsKey(priority))
            {
                agentGoals[priority] = goal;
            }
            else
            {
                agentGoals.Add(priority, goal);
            }
        }

        public void RemoveGoal(AstarGoapGoal goal)
        {
            int keyToRemove = -1;
            foreach (var agentGoal in agentGoals)
            {
                if (agentGoal.Value.Equals(goal))
                {
                    keyToRemove = agentGoal.Key;
                }
            }

            if (keyToRemove > -1)
            {
                agentGoals.Remove(keyToRemove);
            }
        }
        public  IList<AstarGoapAction> GetAgentActions() => agentActions;

        public AstarGoapState GetGoalStateToPlanFor()
        {
            var goal = agentGoals.Values
                .Where(g => g.ShouldPursueGoapGoal(currentState))
                .FirstOrDefault();

            return goal == null ? new AstarGoapState("Empty Goal") : goal.GoalState;
        }

        public void SetFact(string factName, bool factValue) => currentState.SetFact(factName, factValue);
    }
}