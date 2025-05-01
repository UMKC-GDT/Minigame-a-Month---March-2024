using UnityEngine;

namespace BehaviorTree {
    public class Blackboard {
        public GameObject enemy;
        public bool enemyGrabbed;
        public Transform agent;
        public float visionRange = 10f;
        public float grabRange = 5f;
    }
}
