using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BossArmController : MonoBehaviour {
    private float grabAttemptStartTime;
    private float maxGrabDuration = 3.0f;

    public GameObject player, bossesHand;
    public Graph navigationalGraph;
    public LineRenderer lineRenderer;
    public Locations ambientWaypoints;
    public LayerMask layerMask;

    private Node previousNode, nextNode, lastNearbyNodeToPlayer;
    private List<Node> pathTraveled, currentPath;

    public UnityEvent onBossBeginsChasing, onBossCaughtYou, onBossPulledYouBack;

    [SerializeField] private bool isLerping = false;
    private float startTime = 0, travelTime = 0;
    private bool grabbedPlayer = false, grabbingPlayer = false, pullingPlayerBack = false, isBackToTraversePath = false, skipFirstReturnNode = true;

    public Node bossesDoorNode;
    public float bossesSpeed = 1.0f;
    public float bossAttackRange = 10.0f;

    private List<Vector3> armPositions;
    private Vector3 playerGrabbedPosition = Vector3.zero;

    private void Start() {
        armPositions = new();
        ClearLineRenderer();
        navigationalGraph.nodes.Repopulate();
        navigationalGraph.ContstructGraph();
    }

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
        pathTraveled = pathTraveled ?? new List<Node>();
        pathTraveled.Clear();
    }

    private void SetNextPositions() {
        Debug.Log($"[SetNextPositions] isLerping: {isLerping}, grabbing: {grabbingPlayer}, grabbed: {grabbedPlayer}");

        if (!grabbingPlayer && !grabbedPlayer &&
            Vector3.Distance(player.transform.position, bossesHand.transform.position) < bossAttackRange) {
            GrabPlayer();
        } else {
            UpdateClosestNodeToPlayer();
            previousNode = pathTraveled.Last();
            currentPath = navigationalGraph.FindPath(previousNode, lastNearbyNodeToPlayer);

            if (currentPath != null && currentPath.Count > 1) {
                nextNode = currentPath[1];
                if (!isLerping && previousNode != null && nextNode != null) {
                    startTime = Time.time;
                    travelTime = Vector3.Distance(previousNode.transform.position, nextNode.transform.position) / bossesSpeed;
                    isLerping = true;
                }
            } else if (currentPath == null && lastNearbyNodeToPlayer == null && !grabbingPlayer && !grabbedPlayer) {
                StartCoroutine(KeepTryingToFindPath());
            }
        }
    }

    private IEnumerator KeepTryingToFindPath() {
        yield return new WaitForSeconds(1.0f);
        SetNextPositions();
    }

    private void GrabPlayer() {
        Debug.Log("Grabbing Player");
        grabbingPlayer = true;
        grabAttemptStartTime = Time.time;
        previousNode = pathTraveled.Last();

        if (!isLerping) {
            startTime = Time.time;
            travelTime = Vector3.Distance(previousNode.transform.position, player.transform.position) / bossesSpeed;
            isLerping = true;
        }
    }

    private void PullPlayerBackToDoor() {
        Debug.Log("Pulling player back to the door");
        pullingPlayerBack = true;
        isLerping = true;}

    private void UpdateReturnPath() {
        if (pathTraveled.Count < 2) {
            Debug.Log("Return path complete");
            onBossPulledYouBack.Invoke();
            return;
        }

        previousNode = pathTraveled[^1];
        nextNode = pathTraveled[^2];

        if(skipFirstReturnNode == false) {
            pathTraveled.RemoveAt(pathTraveled.Count - 1);
        }
        else {
            skipFirstReturnNode = false;
            startTime = Time.time;
            travelTime = Vector3.Distance(previousNode.transform.position, nextNode.transform.position) / bossesSpeed;
            isLerping = true;
        }

        if (!isLerping && previousNode != null && nextNode != null) {
            startTime = Time.time;
            travelTime = Vector3.Distance(previousNode.transform.position, nextNode.transform.position) / bossesSpeed;
            isLerping = true;
        }
    }

    private void FixedUpdate() {
        if (pullingPlayerBack) {
            HandleReturnPath();
        } else if (grabbingPlayer) {
            HandleGrabbing();
        } else if (isLerping) {
            HandleChase();
        }
    }

    private void HandleGrabbing() {
        bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, player.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));

        if (Time.time - grabAttemptStartTime > maxGrabDuration) {
            Debug.LogWarning("Grab failed — timeout");
            isLerping = false;
            grabbingPlayer = false;
            StartCoroutine(RetryGrabSoon());
            return;
        }

        if (Vector3.Distance(bossesHand.transform.position, player.transform.position) < 0.001f) {
            Debug.Log("Grabbed the player");
            onBossCaughtYou.Invoke();
            player.gameObject.transform.parent = bossesHand.transform;
            player.GetComponent<PolygonCollider2D>().enabled = false;
            isLerping = false;
            grabbingPlayer = false;
            grabbedPlayer = true;
            UpdateLineTo(bossesHand.transform.position);
            playerGrabbedPosition = bossesHand.transform.position;
            PullPlayerBackToDoor();
        }
    }

    private void HandleChase() {
        if (nextNode == null || previousNode == null) return;

        bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, nextNode.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
        UpdateLineTo(bossesHand.transform.position);

        if (Vector3.Distance(bossesHand.transform.position, nextNode.transform.position) < 0.001f) {
            isLerping = false;
            pathTraveled.Add(nextNode);
            SetNextPositions();
        }
    }

    bool skipTheFirstReturnPath = true;
    private void HandleReturnPath() {
        if (!isBackToTraversePath)
        {
            bossesHand.transform.position = Vector3.Lerp(playerGrabbedPosition, pathTraveled.Last().transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));
            UpdateLineTo(bossesHand.transform.position);

            if (Vector3.Distance(bossesHand.transform.position, pathTraveled.Last().transform.position) < 0.001f)
            {
                Debug.Log("Reached the path — start full return");
                isLerping = false;
                isBackToTraversePath = true;
            }
        } else {
            if (nextNode == null || previousNode == null) return;

            bossesHand.transform.position = Vector3.Lerp(previousNode.transform.position, nextNode.transform.position, Mathf.Min(1, (Time.time - startTime) / travelTime));

            lineRenderer.positionCount = 0;
            armPositions = navigationalGraph.NodesListToVector3List(pathTraveled.GetRange(0, pathTraveled.Count - 1));
            armPositions.Add(bossesHand.transform.position);
            lineRenderer.positionCount = armPositions.Count;
            lineRenderer.SetPositions(armPositions.ToArray());

            if (Vector3.Distance(bossesHand.transform.position, nextNode.transform.position) < 0.001f && skipTheFirstReturnPath == false)
            {
                Debug.Log("Reached return node. Removing Node: " + pathTraveled.ElementAt<Node>(pathTraveled.Count - 1));
                isLerping = false;
                UpdateReturnPath();
            }
        }
    }

    private IEnumerator RetryGrabSoon() {
        yield return new WaitForSeconds(0.5f);
        SetNextPositions();
    }

    private void UpdateLineTo(Vector3 handPosition) {
        lineRenderer.positionCount = 0;
        armPositions = navigationalGraph.NodesListToVector3List(pathTraveled);
        armPositions.Add(handPosition);
        lineRenderer.positionCount = armPositions.Count;
        lineRenderer.SetPositions(armPositions.ToArray());
    }

    private void UpdateClosestNodeToPlayer() {
        Node closest = FindClosestNodeToPlayer();
        if (closest != null)
            lastNearbyNodeToPlayer = closest;
    }

    private Node FindClosestNodeToPlayer() {
        return navigationalGraph.FindAdjacentNodes(player.transform.position, "AmbientWaypoint", 100, layerMask);
    }

    private void ClearLineRenderer() {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player")) {
            onBossCaughtYou.Invoke();
            isLerping = false;
            grabbedPlayer = true;
            grabbingPlayer = false;
        }
    }
}
