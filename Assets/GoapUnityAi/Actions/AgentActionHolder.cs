using System.Collections.Generic;
using UnityEngine;

namespace GoapUnityAi
{
    public class AgentActionHolder : MonoBehaviour
    {
        public List<ActionBehaviour> agentActions;

        private IDictionary<string, ActionBehaviour> actionMap;

        private void Start()
        {
            actionMap = new Dictionary<string, ActionBehaviour>();

            agentActions.ForEach(a => actionMap.Add(a.ActionName, a));
        }

        public List<ActionBehaviour> GetAllActions()
        {
            return agentActions;
        }

        public ActionBehaviour GetAction(string actionName)
        {
            if(actionMap.ContainsKey(actionName))
            {
                return actionMap[actionName];
            }
            return null;
        }

        public ActionBehaviour GetAction<T>() where T : ActionBehaviour
        {
            foreach (var action in agentActions)
            {
                if (action is T) return action;
            }
            return null;
        }
    }
}
