using UnityEngine;

namespace BehaviorTree {
    public class ReturnToOffice : BTNode {
        public ReturnToOffice(Vector2 position) : base(position) { }

        public override NodeState Tick() {
            // Do something useful
            return NodeState.Success;
        }
    }
}
