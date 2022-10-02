using System.Collections.Generic;

namespace GoapAi
{
    public interface IUseGoapPlan<A, T>
        where A : IGoapAction<A, T>
        where T : IState<T>
    {
        T GetCurrentState();
        T GetGoalStateToPlanFor();
        IList<A> GetAgentActions();
    }
}