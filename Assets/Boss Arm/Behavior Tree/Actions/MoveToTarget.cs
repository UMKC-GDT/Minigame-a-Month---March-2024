using UnityEngine;

namespace BehaviorTree {
    public class MoveToTarget : BTNode {
        public MoveToTarget(Vector2 position) : base(position) { }

        public override NodeState Tick() {
            // Do something useful
            return NodeState.Success;
        }
    }
}
