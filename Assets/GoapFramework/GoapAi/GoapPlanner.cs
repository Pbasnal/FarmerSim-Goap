using System.Collections.Generic;
using InsightsLogger;

namespace GoapAi
{
    public class GoapPlanner<A, T>
        where A : IGoapAction<A, T>
        where T : IState<T>
    {
        private readonly IFindActionPlan<A, T> planFinder;
        private readonly ISimpleLogger logger;


        public GoapPlanner(
            IFindActionPlan<A, T> planFinder,
            ISimpleLogger logger)
        {
            this.planFinder = planFinder;
            this.logger = logger;
        }

        public IList<A> FindPlanForAgent(IUseGoapPlan<A, T> agent)
        {
            return planFinder.FindPlanForAgent(agent);
        }
    }
}