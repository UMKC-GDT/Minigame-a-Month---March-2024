using UnityEngine;

namespace BehaviorTree {
    public class InverterNode : BTNode {
        public InverterNode(Vector2 position) : base(position) { }

        protected override NodeState OnTick() {
            if (children.Count == 0)
                return NodeState.Failure;

            var childState = children[0].Tick();

            switch (childState) {
                case NodeState.Success: return NodeState.Failure;
                case NodeState.Failure: return NodeState.Success;
                case NodeState.Running: return NodeState.Running;
                default: return NodeState.Failure;
            }
        }
    }
}
