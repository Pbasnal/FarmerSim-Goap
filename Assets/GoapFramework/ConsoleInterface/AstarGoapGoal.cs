using ConsoleInterface;
using GoapAi;

namespace ConsoleInterface
{
    public class AstarGoapGoal : IGoapGoal<AstarGoapState>
    {
        public string GoalName { get; }
        public AstarGoapState PreConditions { get; }
        public AstarGoapState GoalState { get; }

        public AstarGoapGoal(string goalName)
        {
            this.GoalName = goalName;
            PreConditions = new AstarGoapState($"{goalName}-conditions");
            GoalState = new AstarGoapState($"{goalName}-state");
        }

        public void SetAPreconditionFact(string factName, bool factValue) => PreConditions.SetFact(factName, factValue);
        public void SetAGoalFact(string factName, bool factValue) => GoalState.SetFact(factName, factValue);
        public bool ShouldPursueGoapGoal(AstarGoapState currentState) => PreConditions.IsSameAs(currentState);
    }
}