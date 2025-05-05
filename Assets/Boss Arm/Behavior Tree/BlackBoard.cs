using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree {
public class Blackboard {
    // References
    public Transform agent;
    public GameObject enemy;
    public GameObject currentTarget;
    public Node door;
    public GameObject hand;

    // Pathfinding
    public Graph graph;
    public List<Node> currentPath;

    // Behavior Variables
    public float moveSpeed = 50f;
    public float grabRange;
    public float arrivalRange;
    public bool enemyGrabbed;
}
}
