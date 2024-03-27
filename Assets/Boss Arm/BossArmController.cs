using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BossArmController : MonoBehaviour {
    // References
    public GameObject player, bossesHand;
    public Graph navigationalGraph;
    public LineRenderer lineRenderer;
    public Locations ambientWaypoints;
    
    // Public variables
    public LayerMask layerMask;

    // Pathfinding
    private Node previousNode, nextNode, lastNearbyNodeToPlayer;
    private Vector3 currentLastPos, currentNextPos;
    private List<Node> pathTraveled, currentPath;
    
    // Event System
    public UnityEvent onBossCaughtYou;

    // State variables
    private bool isLerping = false;
    private float startTime = 0, travelTime = 0;
    private bool grabbedPlayer = false;

    // Game Designers can tune this to change the boss' behavior
    public Node bossesDoorNode;
    public float bossesSpeed = 1.0f;
    public float bossesAttackRadge = 10.0f;



    // Initialization on Start
    private void Start() {
        ClearLineRenderer();
        navigationalGraph.nodes.Repopulate();
        navigationalGraph.ContstructGraph();
        StartChaseUnderling();
    }

    public void StartChaseUnderling() {
        ResetPathTraveled();
        pathTraveled.Add(bossesDoorNode);
        grabbedPlayer = false;
        SetNextPositions();
    }

    private void ResetPathTraveled() {
        if (pathTraveled == null)
            pathTraveled = new List<Node>();
        else
            pathTraveled.Clear();
    }
    
    private void SetNextPositions()
    {
        LogPrevAndNextNode();
        if(grabbedPlayer == false)
        {
            Debug.Log("Chasing player");
            // Either get a new path and move to the second node in it, or just go straight for the player
            if(Vector3.Distance(player.transform.position, bossesHand.transform.position) < bossesAttackRadge) {
                // Attack the player
                Debug.Log("Within bonking distance");
            }
            else { // Find a path and go to the next spot in it
                Debug.Log("Finding path to player's last nearby node");
                UpdateClosestNodeToPlayer();
                // Begin from the next node
                previousNode = pathTraveled.Last();
                // Find a path to the player
                currentPath = navigationalGraph.FindPath(previousNode, lastNearbyNodeToPlayer);
                // If there is a path and it is longer than 1
                if (currentPath != null && currentPath.Count > 1)
                {
                    Debug.Log("Path found");
                    nextNode = currentPath[1];
                    // For debugging
                    DrawPathToEmployee();
                    // Movebetween hand currentNode and nextNode
                    StartMovingBetweenPoints(previousNode.transform.position, nextNode.transform.position);
                }
                else
                {
                    // Attack the player
                    Debug.Log("Path too short, begin bonking");

                }
                LogPrevAndNextNode();
            }
        }
    }

    private void FixedUpdate() {
        if(isLerping) {
            LerpHandTowardNextLocation();

            if (ReachedNodeDestination()) {
                isLerping = false;
                SetNextPositions();
            }
        }
    }


    public void StartMovingBetweenPoints(Vector3 A, Vector3 B) {
        if(A == null || B == null) {
            Debug.Log("A or B is null.");
            return;
        }
        startTime = Time.time;
        float dist = Vector3.Distance(previousNode.transform.position, nextNode.transform.position);
        travelTime = dist / bossesSpeed;
        isLerping = true;
    }


    // Utility Functions
    private void UpdateClosestNodeToPlayer()
    {
        Node closestNodeToPlayer = FindClosestNodeToPlayer();
        if (closestNodeToPlayer != null) // If we found a node nearby the player, update the last closest node we've logged
            lastNearbyNodeToPlayer = closestNodeToPlayer;
        else // Assume no end Node exists within sight of the player
            Debug.Log("No nearby node to the player. #BlameRobert");
    }
    private bool ReachedNodeDestination() {
        return (Vector3.Distance(bossesHand.transform.position, currentNextPos) < 0.001f);
    }
    private void LerpHandTowardNextLocation() {
        bossesHand.transform.position = Vector3.Lerp(currentLastPos, currentNextPos, (Time.time - startTime) / travelTime);
    }
    private Node FindClosestNodeToPlayer() {
        return navigationalGraph.FindAdjacentNodes(player.transform.position, "AmbientWaypoint", 100, layerMask);
    }
    public void DrawPathToEmployee() {
        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(navigationalGraph.NodesListToVector3List(currentPath).ToArray());
    }
    private void ClearLineRenderer() {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }
    private void LogPrevAndNextNode()
    {
        if(previousNode != null)
        {
            Debug.Log("Previous node: " + previousNode.name);
        }
        else
        {
            Debug.Log("previous node is null");
        }
        if(nextNode != null)
        {
            Debug.Log("Next node: " + nextNode.name);
        }
        else
        {
            Debug.Log("Next node is null");
        }
    }
}
