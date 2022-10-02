using GoapAi;
using PathfindingAi.Astar;

namespace PathfindingAi.AstarWrapper
{
    public interface IAstarGoapEdge<A, T> : IWeightedEdge, IGoapAction<A, T>
        where T : IState<T>
        where A : IGoapAction<A, T>
    { }
}