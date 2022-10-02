namespace GoapAi
{
    public interface IState<T>
        where T : IState<T>
    {
        string StateName { get; }

        float DistanceFrom(T fromNode);
    }
}
