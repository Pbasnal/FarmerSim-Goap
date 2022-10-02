using System;
using Library.Movement;
using Library.Logging;
using UnityEngine;

namespace GoapUnityAi
{
    [Serializable]
    public class MoveToAction : ActionBehaviour
    {
        public float speed;

        private void Start()
        {
            ActionName = "MoveToAction";
            preconditions.StateName = ActionName + "-conditions";
            effects.StateName = ActionName + "-effects";
        }

        public override int ExecuteAction(GoapAgent agent)
        {
            // clicked position would be on the ground or surface of an object. Agent will be standing on top of it and it's center
            // would be higher than the clicked position. Hence, using a point which is on the plane of object.
            // var currentPosition = agent.transform.position;
            // var distanceOfAgentCenterFromGround = currentPosition.y - agent.targetPosition.y;

            // currentPosition = new Vector3(agent.transform.position.x, agent.targetPosition.y, agent.transform.position.z);

            // var updatedPosition = positionMovement.MoveAgent(currentPosition, agent.targetPosition, speed);
            // if (updatedPosition.HasValue)
            // {
            //     agent.transform.position = new Vector3(updatedPosition.Value.x, updatedPosition.Value.y + distanceOfAgentCenterFromGround, updatedPosition.Value.z);
            //     return 0;
            // }
            return 1;
        }
    }
}
