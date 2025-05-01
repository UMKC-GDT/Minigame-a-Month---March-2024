using UnityEngine;

namespace BehaviorTree {
    public class SelectorNode : BTNode {
    public SelectorNode(Vector2 position) : base(position) { }

    public override NodeState Tick() {
        foreach (var child in children) {
            var result = child.Tick();
            if (result == NodeState.Success) return NodeState.Success;
            if (result == NodeState.Running) return NodeState.Running;
        }
        return NodeState.Failure;
    }
}
}
