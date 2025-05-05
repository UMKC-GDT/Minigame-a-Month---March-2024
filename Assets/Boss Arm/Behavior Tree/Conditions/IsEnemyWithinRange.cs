using UnityEngine;

namespace BehaviorTree {
    public class IsEnemyWithinRange : BTNode {
        private Blackboard blackboard;

        // Used at runtime
        public IsEnemyWithinRange(Vector2 position, Blackboard bb) : base(position) {
            blackboard = bb;
        }

        // Used by the editor (for drawing and saving)
        public IsEnemyWithinRange(Vector2 position) : base(position) {
            // blackboard will be assigned later at runtime
        }

        public void SetBlackboard(Blackboard bb) {
            blackboard = bb;
        }

        protected override NodeState OnTick() {
            if (blackboard == null || blackboard.enemy == null || blackboard.agent == null)
                return NodeState.Failure;

            float distanceToEnemy = Vector2.Distance(
                blackboard.agent.transform.position,
                blackboard.enemy.transform.position
            );

            return distanceToEnemy < blackboard.grabRange ? NodeState.Success : NodeState.Failure;
        }
    }
}
