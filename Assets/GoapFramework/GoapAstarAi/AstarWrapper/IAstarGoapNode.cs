using GoapAi;
using PathfindingAi.Astar;

namespace PathfindingAi.AstarWrapper
{
    public interface IAstarGoapNode<T> : IState<T>, IAstarPathNode
        where T : IAstarGoapNode<T>
    { }
}