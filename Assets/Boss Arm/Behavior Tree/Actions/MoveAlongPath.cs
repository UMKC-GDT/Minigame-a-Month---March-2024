using UnityEngine;

namespace BehaviorTree {
    public class MoveAlongPath : BTNode {
        private Blackboard bb;
        private int pathIndex = 0;
        private int lastPathCount = -1;

        public MoveAlongPath(Vector2 pos, Blackboard blackboard) : base(pos) {
            bb = blackboard;
        }

        public MoveAlongPath(Vector2 position) : base(position) {
            // Will be assigned later
        }

        protected override NodeState OnTick() {
            if (bb.currentPath == null || bb.currentPath.Count == 0)
                return NodeState.Failure;

            // If the path was changed externally, reset index
            if (lastPathCount != bb.currentPath.Count) {
                pathIndex = 0;
                lastPathCount = bb.currentPath.Count;
            }

            if (pathIndex >= bb.currentPath.Count) {
                return Vector3.Distance(bb.hand.transform.position, bb.currentTarget.transform.position) <= bb.grabRange ? NodeState.Failure : NodeState.Running;
            }

            Node targetNode = bb.currentPath[pathIndex];
            float distToNode = Vector3.Distance(bb.hand.transform.position, targetNode.transform.position);
            float distToEnemy = Vector3.Distance(bb.hand.transform.position, bb.currentTarget.transform.position);

            // Exit early if player is closer than the next node
            if (distToEnemy <= distToNode)
                return NodeState.Failure;

            // Move toward the current node
            bb.hand.transform.position = Vector3.MoveTowards(
                bb.hand.transform.position,
                targetNode.transform.position,
                bb.moveSpeed * Time.deltaTime
            );

            if (distToNode < bb.arrivalRange)
                pathIndex++;

            return NodeState.Running;
        }
    }
}
