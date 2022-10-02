using System;
using System.Collections.Generic;

namespace ConsoleInterface
{
    public class MainToTestGoap
    {
        public static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            logger.SetLogLevel(LogLevel.None);

            //! Create an Agent and add the possible actions
            var agent = new Agent(logger);
            agent.AddAction(GetGunAction());
            agent.AddAction(PickUpAction());
            agent.AddAction(ShootAction());

            //! Set a goal for the agent
            var goal = KillEnemyGoal();
            agent.AddGoal(0, goal);

            //! Set current state of the agent
            agent.SetFact("EnemyDead", false);
            agent.SetFact("HasGun", false);
            agent.SetFact("ReachedGun", false);
            agent.SetFact("Walk", true);

            //! Get the plan of the agent
            var goapPlanner = GoapPlannerFactory<AstarGoapAction, AstarGoapState>.GetAGoapPlanner(logger);
            Console.WriteLine("\n Original Plan");
            PrintPath(goapPlanner.FindPlanForAgent(agent), goal);

            //! Change the state of the agent 
            agent.SetFact("EnemyDead", false);
            agent.SetFact("HasGun", true);
            agent.SetFact("ReachedGun", false);
            agent.SetFact("Walk", false);

            //! Get the plan of the agent
            Console.WriteLine("\n Plan for updated state");
            PrintPath(goapPlanner.FindPlanForAgent(agent), goal);
        }

        private static void PrintPath(IList<AstarGoapAction> path, AstarGoapGoal goal)
        {
            Console.WriteLine($"Goap Plan");
            foreach (var step in path)
            {
                var action = step;
                Console.Write($"{action.ActionName} -> ");
            }
            Console.WriteLine($" <{goal.GoalName}>");
        }
        private static AstarGoapAction GetGunAction()
        {
            var getGun = GetAction("getGun");
            getGun.PreConditions.SetFact("Walk", true);
            getGun.Effects.SetFact("ReachedGun", true);
            return getGun;
        }

        private static AstarGoapAction PickUpAction()
        {
            var pickUp = GetAction("pickUp");
            pickUp.PreConditions.SetFact("ReachedGun", true);
            pickUp.Effects.SetFact("HasGun", true);
            return pickUp;
        }

        private static AstarGoapAction ShootAction()
        {
            var shoot = GetAction("shoot");
            shoot.PreConditions.SetFact("HasGun", true);
            shoot.Effects.SetFact("EnemyDead", true);
            return shoot;
        }

        private static AstarGoapAction GetAction(string name)
        {
            return new AstarGoapAction(name)
            {
                PreConditions = new AstarGoapState($"p-{name}"),
                Effects = new AstarGoapState($"e-{name}"),
                Cost = 1
            };
        }

        private static AstarGoapGoal KillEnemyGoal()
        {
            var goal = new AstarGoapGoal("KillEnemy");

            goal.SetAGoalFact("EnemyDead", true);
            return goal;
        }
    }
}