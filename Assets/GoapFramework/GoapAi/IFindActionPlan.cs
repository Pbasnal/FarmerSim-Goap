using System.Collections.Generic;

namespace GoapAi
{
    public interface IFindActionPlan<A, T>
        where T : IState<T>
        where A : IGoapAction<A, T>
    {
        IList<A> FindPlanForAgent(IUseGoapPlan<A, T> agent);
    }
}