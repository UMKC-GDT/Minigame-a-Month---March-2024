using UnityEngine;

namespace BehaviorTree {public class SequenceNode : BTNode {
    public SequenceNode(Vector2 position) : base(position) { }

    public override NodeState Tick() {
        foreach (var child in children) {
            var result = child.Tick();
            if (result == NodeState.Failure) return NodeState.Failure;
            if (result == NodeState.Running) return NodeState.Running;
        }
        return NodeState.Success;
    }
}
}
