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

        public abstract NodeState Tick();

        public void AddChild(BTNode child) {
            if (!children.Contains(child)) children.Add(child);
        }
    }
}
