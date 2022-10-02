using Library.Logging;
using UnityEngine;

namespace GoapUnityAi
{
    public class SleepAction : ActionBehaviour
    {
        private SmartObject bed;

        private void Start()
        {
            bed = FindObjectOfType<Bed>();
            ActionName = "SleepAction";
            preconditions.StateName = ActionName + "-conditions";
            effects.StateName = ActionName + "-effects";
        }

        public override bool CanReachStateWithAction(AgentState resultingState)
        {
            return base.CanReachStateWithAction(resultingState) && bed != null;
        }

        public override int ExecuteAction(GoapAgent agent)
        {
            if (!isAgentInRangeOfBed(agent))
            {
                return 0;
            }
            DebugLogger.LogDebug($"Sleeping!! zzzz");

            if (agent.GetCurrentState().GetFact(GoapFactName.TimeToWork))
            {
                agent.energy = 20;
                agent.GetCurrentState().SetFact(GoapFactName.HasEnergy, true);
                return 1;
            }
            return 0;
        }

        private bool isAgentInRangeOfBed(GoapAgent agent)
        {
            return agent.MoveTowards(bed.transform.position);
        }
    }
}
