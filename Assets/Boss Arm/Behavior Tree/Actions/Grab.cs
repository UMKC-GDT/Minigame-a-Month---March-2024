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

    protected override NodeState OnTick() {
        if (blackboard == null || blackboard.enemy == null)
            return NodeState.Failure;

        float dist = Vector3.Distance(blackboard.hand.transform.position, blackboard.enemy.transform.position);
        Debug.Log($"[Grab] Distance to enemy: {dist}, GrabRange: {blackboard.grabRange}");

        if (dist > blackboard.grabRange)
            return NodeState.Failure;

        blackboard.enemyGrabbed = true;
        Debug.Log("Enemy grabbed!");
        return NodeState.Success;
    }
}
}
