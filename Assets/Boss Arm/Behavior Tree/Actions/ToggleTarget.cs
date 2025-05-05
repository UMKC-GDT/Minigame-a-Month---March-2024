using UnityEngine;

namespace BehaviorTree {
    public class ToggleTarget : BTNode
    {    
        private Blackboard bb;

        public ToggleTarget(Vector2 pos, Blackboard blackboard) : base(pos) {
            bb = blackboard;
        }

        public ToggleTarget(Vector2 position) : base(position)
        {
        }

        protected override NodeState OnTick()
        {
            bb.currentTarget = bb.door.transform.gameObject;
            return NodeState.Success;
        }
    }
}
