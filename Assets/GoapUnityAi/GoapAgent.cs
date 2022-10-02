using System;
using System.Collections.Generic;
using GameSystems.GameTime;
using GoapAi;
using GoapAstarAi;
using Library.Logging;
using Library.Movement;
using PathfindingAi.Astar;
using UnityEngine;

namespace GoapUnityAi
{
    public abstract class GoapAgent : MonoBehaviour, IUseGoapPlan<ActionBehaviour, AgentState>
    {
        public ActionBehaviour actionToExecute;

        public AgentState agentState;

        public PrioritizedAgentGoals prioritizedAgentGoals;
        public AgentActionHolder actionHolder;

        /// MoveTo action related fields
        public float speed = 1.0f;

        /// WorkInField related fields
        public float energy = 100;

        // Inventory
        public FieldTool fieldTool;

        private GoapPlanner<ActionBehaviour, AgentState> goapPlanner;

        private bool hasPlan = false;

        private GameTimeSystem gameTimeSystem;

        private List<ActionBehaviour> plan;

        private void Awake()
        {
            var logger = DebugLogger.GetLogger();
            var frontier = new SortedSetFrontier(logger);
            var astarGraph = new AstarGraph(frontier, logger);

            var factsHeuristics = new FactsHeuristicCalculator();
            var pathFinder = new AstarAlgorithmWrapper<ActionBehaviour, AgentState>(astarGraph, factsHeuristics, logger);

            goapPlanner = new GoapPlanner<ActionBehaviour, AgentState>(pathFinder, logger);

            plan = new List<ActionBehaviour>();

            gameTimeSystem = FindObjectOfType<GameTimeSystem>();
        }

        private void Update()
        {
            var currentGameTime = gameTimeSystem.GetCurrentTimeInfo();
            // DebugLogger.LogDebug($"Current time of day: {currentGameTime.TimeOfDay.ToString()}");

            if (!hasPlan)
            {
                IList<ActionBehaviour> goapPlan = CreateAGoapPlan();
                SetUnityAgentPlanWithGoapPlan(goapPlan);
            }

            if (hasPlan)
            {
                ExecutePlan(plan);
            }
        }

        public virtual bool MoveTowards(Vector3 targetPosition)
        {
            // clicked position would be on the ground or surface of an object. Agent will be standing on top of it and it's center
            // would be higher than the clicked position. Hence, using a point which is on the plane of object.
            var currentPosition = transform.position;
            var distanceOfAgentCenterFromGround = currentPosition.y - targetPosition.y;

            currentPosition = new Vector3(currentPosition.x, targetPosition.y, currentPosition.z);

            var updatedPosition = PositionMovement.MoveAgent(currentPosition, targetPosition, speed);
            var hasReachedTargetPosition = !updatedPosition.HasValue;
            
            if (!hasReachedTargetPosition)
            {
                transform.position = new Vector3(updatedPosition.Value.x,
                    updatedPosition.Value.y + distanceOfAgentCenterFromGround,
                    updatedPosition.Value.z);
            }

            return hasReachedTargetPosition;
        }

        private void ExecutePlan(List<ActionBehaviour> plan)
        {
            if (actionToExecute == null)
            {
                actionToExecute = plan[0];
            }

            var actionCompleted = actionToExecute.ExecuteAction(this) == 1;

            if (actionCompleted)
            {
                actionToExecute.UpdateAgentStateWithEffects(agentState);
                actionToExecute = null;
                plan.RemoveAt(0);
            }

            hasPlan = plan.Count > 0;
        }

        private IList<ActionBehaviour> CreateAGoapPlan()
        {
            var plan = goapPlanner.FindPlanForAgent(this);
            hasPlan = plan != null && plan.Count > 0;

            if (hasPlan)
            {
                var goal = GetGoalStateToPlanFor();
                var planStr = "";
                foreach (var step in plan)
                {
                    planStr += step.ActionName + " -> ";
                }
                DebugLogger.LogDebug($"Plan for goal {goal.StateName} is {planStr}");
            }
            return plan;
        }

        private void SetUnityAgentPlanWithGoapPlan(IList<ActionBehaviour> goapPlan)
        {
            plan.Clear();

            if (!hasPlan)
            {
                return;
            }

            foreach (var action in goapPlan)
            {
                plan.Add(actionHolder.GetAction(action.ActionName));
            }
        }

        public void SetActionToExecute(ActionBehaviour action)
        {
            // DebugLogger.LogDebug($"Setting action to {action}");
            actionToExecute = action;
        }

        public AgentState GetCurrentState()
        {
            return agentState;
        }

        public AgentState GetGoalStateToPlanFor()
        {
            var unityGoal = prioritizedAgentGoals.GetGoal(GetCurrentState());
            return unityGoal?.GoalState;
        }

        public ActionBehaviour GetAction<T>() where T : ActionBehaviour
        {
            var action = actionHolder.GetAction<T>();
            // DebugLogger.LogDebug($"Checking for agent Action: {action.ActionName}");
            return action;
        }

        public IList<ActionBehaviour> GetAgentActions()
        {
            // foreach (var action in actionHolder.GetAllActions())
            // {
            //     DebugLogger.LogDebug($"Agent Action: {action.ActionName}");
            // }

            return actionHolder.GetAllActions();
        }
    }
}
