using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using System.Collections;

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
    private List<Node> pathTraveled, currentPath;
    
    // Event System
    public UnityEvent onBossBeginsChasing, onBossCaughtYou, onBossPulledYouBack;

    // State variables
    [SerializeField]
    private bool isLerping = false;
    private float startTime = 0, travelTime = 0;
    private bool grabbedPlayer = false, grabbingPlayer = false, pullingPlayerBack = false, isBackToTraversePath = false;

    // Game Designers can tune this to change the boss' behavior
    public Node bossesDoorNode;
    public float bossesSpeed = 1.0f;
    public float bossesAttackRadge = 10.0f;
    List<Vector3> armPositions;
    Vector3 playerGrabbedPosition = Vector3.zero;


    // Initialization on Start
    private void Start() {
        armPositions = new();
        ClearLineRenderer();
        navigationalGraph.nodes.Repopulate();
        navigationalGraph.ContstructGraph();
        // StartChaseUnderling();
    }

    // Resets everything and begins the chase from the boss's door
    public void StartChaseUnderling() {
        ResetPathTraveled();
        pathTraveled.Add(bossesDoorNode);
        grabbedPlayer = false;
        grabbingPlayer = false;
        pullingPlayerBack = false;
        isBackToTraversePath = false;
        onBossBeginsChasing.Invoke();
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
        Debug.Log("Chasing player");
        // If player is close enough to the hand to be grabbed
        if(Vector3.Distance(player.transform.position, bossesHand.transform.position) < bossesAttackRadge) {
            // Attack the player
            Debug.Log("Within bonking distance");
            GrabPlayer();
        } else { // Find a path and go to the next spot in it
            //Debug.Log("Finding path to player's last nearby node");
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
                // Movebetween hand currentNode and nextNode
                if (isLerping == false && previousNode != null && nextNode != null)
                {
                    startTime = Time.time;
                    travelTime = Vector3.Distance(previousNode.transform.position, nextNode.transform.position) / bossesSpeed;
                    isLerping = true;
                }
            }
            else if(currentPath != null)
            {
                // Attack the player
                Debug.Log("Path too short, begin bonking");
            }
            if(currentPath == null && lastNearbyNodeToPlayer == null && grabbingPlayer == false && grabbedPlayer == false)
            {
                StartCoroutine(KeepTryingToFindPath());
            }
            LogPrevAndNextNode();
        }
    }

    private IEnumerator KeepTryingToFindPath()
    {
        yield return new WaitForSeconds(1.0f);
        SetNextPositions();
    }

    private void GrabPlayer()
    {
        Debug.Log("Grabbing Player");
        grabbingPlayer = true;
        previousNode = pathTraveled.Last();
        if (isLerping == false)
        {
            startTime = Time.time;
            travelTime = Vector3.Distance(previousNode.transform.position, player.transform.position) / bossesSpeed;
            isLerping = true;
        }
    }

    private void PullPlayerBackToDoor()
    {
        Debug.Log("Pulling player back to the door");
        pullingPlayerBack = true;
        isLerping = true;
    }

    private void UpdateReturnPath()
    {
        previousNode = pathTraveled[pathTraveled.Count - 1];
        nextNode = pathTraveled[pathTraveled.Count - 2];
        if (isLerping == false && previousNode != null && nextNode != null)
        {
            startTime = Time.time;
            travelTime = Vector3.Distance(previousNode.transform.position, nextNode.transform.position) / bossesSpeed;
            isLerping = true;
        }
    }

    private void FixedUpdate() {
        if(pullingPlayerBack)
        {
            if (isLerping)
            {
                if(isBackToTraversePath == false)
                {
                    bossesHand.transform.position = Vector3.Lerp(playerGrabbedPosition, pathTraveled.Last().transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
                    lineRenderer.positionCount = 0;
                    armPositions = navigationalGraph.NodesListToVector3List(pathTraveled);
                    armPositions.Add(bossesHand.transform.position);
                    lineRenderer.positionCount = armPositions.Count;
                    lineRenderer.SetPositions(armPositions.ToArray());
                    if (Vector3.Distance(bossesHand.transform.position, pathTraveled.Last().transform.position) < 0.001f)
                    {
                        if (nextNode == bossesDoorNode)
                        {
                            Debug.Log("Hurray!");
                            onBossPulledYouBack.Invoke();
                            return;
                        }
                        Debug.Log("Reached the path");
                        isLerping = false;
                        UpdateReturnPath();
                        isBackToTraversePath = true;
                    }
                }
                else
                {
                    if (nextNode == null || previousNode == null)
                        return;
                    // Debug.Log("Lerping: " + (Time.time - startTime) / travelTime);
                    // Lerp hand to next destination
                    bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, nextNode.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
                    lineRenderer.positionCount = 0;
                    armPositions = navigationalGraph.NodesListToVector3List(pathTraveled.GetRange(0, pathTraveled.Count - 1));
                    armPositions.Add(bossesHand.transform.position);
                    lineRenderer.positionCount = armPositions.Count;
                    lineRenderer.SetPositions(armPositions.ToArray());
                    // If we reached the destination
                    if (Vector3.Distance(bossesHand.transform.position, nextNode.transform.position) < 0.001f)
                    {
                        if(nextNode == bossesDoorNode)
                        {
                            Debug.Log("Hurray!");
                            onBossPulledYouBack.Invoke();
                            return;
                        }
                        Debug.Log("Reached the nextNode");
                        isLerping = false;
                        pathTraveled.RemoveAt(pathTraveled.Count - 1);
                        UpdateReturnPath();
                    }
                }
            }
        }
        else
        {
            if (grabbingPlayer)
            {
                if (isLerping)
                {
                    bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, player.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
                    if (Vector3.Distance(bossesHand.transform.position, player.transform.position) < 0.001f)
                    {
                        Debug.Log("Grabbed the player");
                        onBossCaughtYou.Invoke();
                        player.gameObject.transform.parent = bossesHand.transform;
                        player.GetComponent<PolygonCollider2D>().enabled = false;
                        isLerping = false;
                        grabbingPlayer = false;
                        grabbedPlayer = true;
                        lineRenderer.positionCount = 0;
                        armPositions = navigationalGraph.NodesListToVector3List(pathTraveled);
                        armPositions.Add(bossesHand.transform.position);
                        lineRenderer.positionCount = armPositions.Count;
                        lineRenderer.SetPositions(armPositions.ToArray());
                        playerGrabbedPosition = transform.position;
                        PullPlayerBackToDoor();
                    }
                }
            }
            else
            {
                if (isLerping)
                {
                    if (nextNode == null || previousNode == null)
                        return;
                    // Debug.Log("Lerping: " + (Time.time - startTime) / travelTime);
                    // Lerp hand to next destination
                    bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, nextNode.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
                    lineRenderer.positionCount = 0;
                    armPositions = navigationalGraph.NodesListToVector3List(pathTraveled);
                    armPositions.Add(bossesHand.transform.position);
                    lineRenderer.positionCount = armPositions.Count;
                    lineRenderer.SetPositions(armPositions.ToArray());
                    // If we reached the destination
                    if (Vector3.Distance(bossesHand.transform.position, nextNode.transform.position) < 0.001f)
                    {
                        Debug.Log("Reached the nextNode");
                        isLerping = false;
                        pathTraveled.Add(nextNode);
                        SetNextPositions();
                    }
                }
            }
        }
    }

    // Utility Functions
    private void UpdateClosestNodeToPlayer()
    {
        Node closestNodeToPlayer = FindClosestNodeToPlayer();
        if (closestNodeToPlayer != null) // If we found a node nearby the player, update the last closest node we've logged
            lastNearbyNodeToPlayer = closestNodeToPlayer;
        // else // Assume no end Node exists within sight of the player
            //Debug.Log("No nearby node to the player. #BlameRobert");
    }
    private Node FindClosestNodeToPlayer() {
        return navigationalGraph.FindAdjacentNodes(player.transform.position, "AmbientWaypoint", 100, layerMask);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            onBossCaughtYou.Invoke();
            isLerping = false;
            grabbedPlayer = true;
            grabbingPlayer = false;
        }
    }
}
