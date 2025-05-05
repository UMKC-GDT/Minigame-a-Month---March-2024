using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree {
public abstract class BTNode {
    public Rect rect;
    public string title;
    public List<BTNode> children = new List<BTNode>();
    public NodeState State { get; protected set; } = NodeState.Running;

    public BTNode(Vector2 position) {
        rect = new Rect(position.x, position.y, 150, 50);
        title = GetType().Name;
    }

    public NodeState Tick() {
        // Debug.Log($"[BTNode] {title} starting Tick()");
        var result = OnTick();
        // Debug.Log($"[BTNode] {title} returned: {result}");
        return result;
    }

    // This is what children override now
    protected abstract NodeState OnTick();

    public void AddChild(BTNode child) {
        if (!children.Contains(child)) children.Add(child);
    }
}
}
