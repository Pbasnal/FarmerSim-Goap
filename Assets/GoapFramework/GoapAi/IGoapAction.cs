namespace GoapAi
{
    public interface IGoapAction<A, T>
        where T : IState<T>
        where A : IGoapAction<A, T>
    {
        string ActionName { get; }
        T PreConditions { get; }
        T Effects { get; }

        bool CanReachStateWithAction(T resultingState);
    }
}