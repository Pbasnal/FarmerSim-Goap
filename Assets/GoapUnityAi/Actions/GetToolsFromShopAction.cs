using Library.Logging;

namespace GoapUnityAi
{
    public class GetToolsFromShopAction : ActionBehaviour
    { 
        private SmartObject toolsShop;

        private void Start()
        {
            toolsShop = FindObjectOfType<ToolsShop>();
            ActionName = "GetToolsAction";
            preconditions.StateName = ActionName + "-conditions";
            effects.StateName = ActionName + "-effects";
        }

        public override bool CanReachStateWithAction(AgentState resultingState)
        {
            return base.CanReachStateWithAction(resultingState) && toolsShop != null;
        }

        public override int ExecuteAction(GoapAgent agent)
        {
            if (!isAgentInRangeOfToolsShop(agent))
            {
                return 0;
            }

            agent.fieldTool.RestoreHp(100);
            agent.GetCurrentState().SetFact(GoapFactName.HasToolsToWork, true);

            DebugLogger.LogDebug($"Buying tools!!");
            return 1;
        }

        private bool isAgentInRangeOfToolsShop(GoapAgent agent)
        {
            return agent.MoveTowards(toolsShop.transform.position);
        }
    }
}
