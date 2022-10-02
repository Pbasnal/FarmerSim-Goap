using System;
using System.Collections;
using Library.Logging;
using UnityEngine;

namespace GoapUnityAi
{
    [Serializable]
    public class WorkInFieldAction : ActionBehaviour
    {
        private GoapAgent agent;
        private SmartObject workField;
        private System.Random randomNumberGenerator;

        private void Start()
        {
            ActionName = "WorkInField";
            preconditions.StateName = ActionName + "-conditions";
            effects.StateName = ActionName + "-effects";

            workField = FindObjectOfType<WorkField>();

            // DebugLogger.LogDebug($"Found a work field: {workField.name}");
            randomNumberGenerator = new System.Random();
        }

        public override bool CanReachStateWithAction(AgentState resultingState)
        {
            // DebugLogger.LogDebug($"Workfield: Can reach goal : {base.CanReachStateWithAction(resultingState)}");
            // DebugLogger.LogDebug($"Workfield: Workfield object : {workField.name}");
            return base.CanReachStateWithAction(resultingState) && workField != null;
        }

        public override int ExecuteAction(GoapAgent agent)
        {
            this.agent = agent;
            // var workField = GetAWorkField();
            // DebugLogger.LogDebug($"Executing work in field action - workfield: {workField.name}");
            if (!PreConditions.IsSameAs(agent.GetCurrentState()))
            {
                // DebugLogger.LogDebug($"Finished work in field action WorkField {workField} | energy {agent.energy}");
                return 1;
            }

            if (!isAgentInRangeOfWorkField(agent))
            {
                return 0;
            }
            UseUpEnergy(agent);
            return 0;
        }

        private bool isAgentInRangeOfWorkField(GoapAgent agent) => agent.MoveTowards(workField.transform.position);

        private WorkField GetAWorkField()
        {
            var workFields = GameObject.FindObjectsOfType<WorkField>();
            if (workFields == null || workFields.Length == 0)
            {
                return null;
            }
            return workFields[0];
        }

        private void UseUpEnergy(GoapAgent agent)
        {
            var damageToTool = randomNumberGenerator.Next(2);
            agent.fieldTool.TakeDamage(damageToTool * Time.deltaTime * 10);
            if (agent.fieldTool.HP == 0)
            {
                DebugLogger.LogDebug("broke tool");
                agent.GetCurrentState().SetFact(GoapFactName.HasToolsToWork, false);
            }

            agent.energy -= Time.deltaTime * 2;
            if (agent.energy < 0)
            {
                agent.GetCurrentState().SetFact(GoapFactName.HasEnergy, false);
            }
        }
    }
}