namespace GoapAi
{
    public interface IGoapGoal<T>
        where T : IState<T>
    {
        string GoalName { get; }
        T PreConditions { get; }
        T GoalState { get; }

        bool ShouldPursueGoapGoal(T currentState);
    }
}