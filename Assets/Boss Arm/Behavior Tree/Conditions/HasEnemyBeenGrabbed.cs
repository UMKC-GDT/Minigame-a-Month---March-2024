using UnityEngine;

namespace BehaviorTree {
public class HasEnemyBeenGrabbed : BTNode {
    private Blackboard blackboard;

    public HasEnemyBeenGrabbed(Vector2 position, Blackboard bb) : base(position) {
        blackboard = bb;
    }

    public HasEnemyBeenGrabbed(Vector2 position) : base(position) {
    // blackboard will be assigned later at runtime
    }

    protected override NodeState OnTick() {
        return blackboard.enemyGrabbed ? NodeState.Success : NodeState.Failure;
    }
}
}
