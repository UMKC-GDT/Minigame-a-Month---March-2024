using UnityEngine;

namespace BehaviorTree {
    public class IsNextNodeCloserThanEnemy : BTNode {
        private Blackboard bb;

        public IsNextNodeCloserThanEnemy(Vector2 position, Blackboard blackboard) : base(position) {
            bb = blackboard;
        }

        public IsNextNodeCloserThanEnemy(Vector2 position) : base(position) {
            // Will be assigned later
        }

        public void SetBlackboard(Blackboard blackboard) {
            bb = blackboard;
        }

        protected override NodeState OnTick() {
            if (bb.currentPath == null || bb.currentPath.Count == 0) return NodeState.Failure;

            Vector3 handPos = bb.hand.transform.position;
            float distToEnemy = Vector3.Distance(handPos, bb.enemy.transform.position);
            float distToNextNode = Vector3.Distance(handPos, bb.currentPath[0].transform.position);

            return distToNextNode < distToEnemy ? NodeState.Success : NodeState.Failure;
        }
    }
}
