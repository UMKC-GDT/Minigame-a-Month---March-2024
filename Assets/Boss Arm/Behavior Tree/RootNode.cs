using UnityEngine;

namespace BehaviorTree {
public class RootNode : BTNode {
    public RootNode(Vector2 position) : base(position) { }

    protected override NodeState OnTick() {
        return children.Count > 0 ? children[0].Tick() : NodeState.Failure;
    }
}
}
