using UnityEngine;

namespace BehaviorTree {
public class ReturnToOffice : BTNode {
    private Blackboard blackboard;

    // Editor constructor
    public ReturnToOffice(Vector2 position) : base(position) { }

    // Runtime constructor
    public ReturnToOffice(Vector2 position, Blackboard bb) : base(position) {
        blackboard = bb;
    }

    public void SetBlackboard(Blackboard bb) {
        blackboard = bb;
    }

    protected override NodeState OnTick() {
        if (blackboard == null || blackboard.door == null || blackboard.hand == null)
            return NodeState.Failure;

        blackboard.hand.transform.position = Vector3.MoveTowards(
            blackboard.hand.transform.position,
            blackboard.door.transform.position,
            Time.deltaTime * blackboard.moveSpeed
        );

        float dist = Vector3.Distance(blackboard.hand.transform.position, blackboard.door.transform.position);
        return dist < 0.5f ? NodeState.Success : NodeState.Running;
    }
}
}
