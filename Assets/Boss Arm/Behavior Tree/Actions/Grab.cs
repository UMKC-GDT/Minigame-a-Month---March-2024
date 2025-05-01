using UnityEngine;

namespace BehaviorTree {
public class Grab : BTNode {
    private Blackboard blackboard;

    // Runtime constructor
    public Grab(Vector2 position, Blackboard bb) : base(position) {
        blackboard = bb;
    }

    // Editor constructor
    public Grab(Vector2 position) : base(position) {
        // Blackboard will be assigned later
    }

    public void SetBlackboard(Blackboard bb) {
        blackboard = bb;
    }

    public override NodeState Tick() {
        if (blackboard == null || blackboard.enemy == null)
            return NodeState.Failure;

        blackboard.enemyGrabbed = true;
        Debug.Log("Enemy grabbed!");
        return NodeState.Success;
    }
}
}
