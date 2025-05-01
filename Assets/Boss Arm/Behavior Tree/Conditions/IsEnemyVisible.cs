using UnityEngine;

namespace BehaviorTree {
public class IsEnemyVisible : BTNode {
    private Blackboard blackboard;

    public IsEnemyVisible(Vector2 position, Blackboard bb) : base(position) {
        blackboard = bb;
    }

    public override NodeState Tick() {
        if (blackboard.enemy == null || blackboard.agent == null) return NodeState.Failure;

        float distance = Vector3.Distance(blackboard.agent.position, blackboard.enemy.transform.position);
        return distance <= blackboard.visionRange ? NodeState.Success : NodeState.Failure;
    }
}
}
