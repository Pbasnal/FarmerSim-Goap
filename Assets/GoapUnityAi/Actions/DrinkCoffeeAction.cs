using System;
using Library.Logging;

namespace GoapUnityAi
{
    public class DrinkCoffeeAction : ActionBehaviour
    {
        private SmartObject coffee;
        private Random randomNumberGenerator;

        private void Start()
        {
            coffee = FindObjectOfType<Coffee>();
            ActionName = "DrinkCoffeeAction";
            preconditions.StateName = ActionName + "-conditions";
            effects.StateName = ActionName + "-effects";
            randomNumberGenerator = new Random();
        }

        public override bool CanReachStateWithAction(AgentState resultingState)
        {
            return base.CanReachStateWithAction(resultingState) && coffee != null;
        }
        public override int ExecuteAction(GoapAgent agent)
        {
            if (!isAgentInRangeOfCoffee(agent))
            {
                return 0;
            }
            agent.energy += randomNumberGenerator.Next(40) + 10;
            agent.GetCurrentState().SetFact(GoapFactName.HasEnergy, true);
            DebugLogger.LogDebug($"Drank coffee!! zzzz");
            return 1;
        }

        private bool isAgentInRangeOfCoffee(GoapAgent agent)
        {
            // var distanceFromBed = Vector3.Distance(agent.transform.position, bed.transform.position);
            // DebugLogger.LogDebug($"Distance from bed {distanceFromBed}");
            return agent.MoveTowards(coffee.transform.position);
        }
    }
}
