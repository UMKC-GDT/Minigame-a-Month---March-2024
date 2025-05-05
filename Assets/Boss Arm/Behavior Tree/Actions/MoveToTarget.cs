using UnityEngine;

namespace BehaviorTree {
    public class MoveToTarget : BTNode {
        private Blackboard blackboard;

        public MoveToTarget(Vector2 position, Blackboard bb) : base(position) {
            blackboard = bb;
        }

        public MoveToTarget(Vector2 position) : base(position) {}

        public void SetBlackboard(Blackboard bb) => blackboard = bb;

        protected override NodeState OnTick() {
            if (blackboard.enemy == null || blackboard.agent == null)
                return NodeState.Failure;

            var agentPos = blackboard.agent.position;
            var targetPos = blackboard.currentTarget.transform.position;

            float distance = Vector3.Distance(agentPos, targetPos);
            Debug.Log($"[MoveToTarget] Distance to enemy: {distance}, ArrivalRange: {blackboard.arrivalRange}");

            blackboard.agent.position = Vector3.MoveTowards(agentPos, targetPos, Time.deltaTime * blackboard.moveSpeed);

            return distance < blackboard.arrivalRange ? NodeState.Success : NodeState.Running;
        }
    }
}
