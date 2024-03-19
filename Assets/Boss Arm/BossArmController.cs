using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BossArmController : MonoBehaviour {
    public LayerMask layerMask;
    public Node bossesDoorNode;
    public LineRenderer lineRenderer;
    public Locations ambientWaypoints;
    public UnityEvent onBossCaughtYou;
    public Graph navigationalGraph;
    public List<Node> nodes;
    public GameObject player;

    private void Start() {
        // Setup the linerenderer
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        ClearLineRenderer();

        // Setup the waypoints
        if(ambientWaypoints == null)
            ambientWaypoints = GameObject.Find("AmbientWaypoints").GetComponent<Locations>();
        if(ambientWaypoints != null)
            if(ambientWaypoints.locations != null)
                if(ambientWaypoints.locations.Count <= 0)
                    ambientWaypoints.Repopulate();
        foreach(GameObject go in ambientWaypoints.locations)
            nodes.Add(go.GetComponent<Node>());
        StartCoroutine(ChaseUnderling());
    }

    public void StartAttack(GameObject player) {

    }

    private void ClearLineRenderer() {
        lineRenderer.positionCount = 0;
    }

    private IEnumerator ChaseUnderling()
    {
        yield return new WaitForSeconds(1.0f);
        GrabEmployee();
        StartCoroutine(ChaseUnderling());
    }
    public void GrabEmployee()
    {
        navigationalGraph.ContstructGraph();
        foreach (GameObject go in ambientWaypoints.locations)
            nodes.Add(go.GetComponent<Node>());
        // Ensure there are at least two nodes to choose from
        if (nodes.Count >= 2)
        {
            Node startNode = bossesDoorNode;
            Node endNode = navigationalGraph.FindAdjacentNodes(player.transform.position, "AmbientWaypoint", 100, layerMask);

            if(endNode == null)
            {
                Debug.Log("No nearby node to the player");
                return;
            }

            List<Node> nodePath = navigationalGraph.FindPath(startNode, endNode);

            // Convert nodePath to Vector3 path for the line renderer
            List<Vector3> path = nodePath.Select(node => node.transform.position).ToList();

            lineRenderer.positionCount = path.Count();
            for (int i = 0; i < path.Count; i++)
                lineRenderer.SetPosition(i, path[i]);
        }
        else
            Debug.LogError("Not enough nodes to perform pathfinding.");
    }

    private IEnumerator GenerateAndDrawRandomPath() {
        yield return new WaitForSeconds(1.0f);
        // Ensure there are at least two nodes to choose from
        if (nodes.Count >= 2) {
            int index1 = Random.Range(0, nodes.Count);
            int index2 = Random.Range(0, nodes.Count);
            // Ensure the second index is different from the first
            while (index1 == index2)
                index2 = Random.Range(0, nodes.Count);
            Node startNode = nodes[index1];
            Node endNode = nodes[index2];
            List<Node> nodePath = navigationalGraph.FindPath(startNode, endNode);
            // Convert nodePath to Vector3 path for the line renderer
            List<Vector3> path = nodePath.Select(node => node.transform.position).ToList();
            lineRenderer.positionCount = path.Count();
            for(int i = 0; i < path.Count; i++)
                lineRenderer.SetPosition(i, path[i]);
        }
        else
            Debug.LogError("Not enough nodes to perform pathfinding.");
        StartCoroutine(GenerateAndDrawRandomPath());
    }
}
