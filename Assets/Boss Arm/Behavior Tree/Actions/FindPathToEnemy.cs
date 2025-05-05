using BehaviorTree;
using UnityEngine;

namespace BehaviorTree {
public class FindPathToEnemy : BTNode {
    private Blackboard bb;

    public FindPathToEnemy(Vector2 pos, Blackboard blackboard) : base(pos) {
        bb = blackboard;
    }

    public FindPathToEnemy(Vector2 position) : base(position) {
        // Will be overwritten later when blackboard is set
    }
    protected override NodeState OnTick() {
        var start = bb.graph.FindAdjacentNodes(bb.hand.transform.position, "AmbientWaypoint", 100, bb.graph.nodeLayerMask);
        var end = bb.graph.FindAdjacentNodes(bb.enemy.transform.position, "AmbientWaypoint", 100, bb.graph.nodeLayerMask);

        if (start != null && end != null) {
            bb.currentPath = bb.graph.FindPath(start, end);
            return bb.currentPath.Count > 0 ? NodeState.Success : NodeState.Failure;
        }

        return NodeState.Failure;
    }
}
}
