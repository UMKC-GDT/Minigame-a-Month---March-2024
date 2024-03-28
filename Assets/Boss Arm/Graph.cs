using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Locations))]
public class Graph : MonoBehaviour
{
    public Locations nodes;
    public float adjacencyRadius;
    public LayerMask nodeLayerMask;
    public string searchTag = "AmbientWaypoint";
    public Dictionary<Node, List<Node>> adjacencyMap;
    public Dictionary<(Node, Node), float> edges;
    public Node start, end;

    private void Start()
    {
        if(nodes == null)
        {
            nodes = GetComponent<Locations>();
        }
    }


    public void ContstructGraph()
    {
        if(adjacencyMap == null)
            adjacencyMap = new();
        if(edges == null)
            edges = new();
        foreach(var node in nodes.locations)
        {
            Node newNode = node.GetComponent<Node>();
            newNode.radius = adjacencyRadius;
            newNode.layerMask = nodeLayerMask;
            newNode.FindAdjacentNodes();
            List<Node> newAdjacentNodes = new();
            foreach(GameObject adjacentNode in newNode.adjacentNodes)
            {
                // Debug.Log("Adjecent to: " + adjacentNode.name);
                newAdjacentNodes.Add(adjacentNode.GetComponent<Node>());
            }
            adjacencyMap[newNode] = newAdjacentNodes;
        }

        foreach(Node node in adjacencyMap.Keys)
        {
            foreach(Node adjacentNode in adjacencyMap[node])
            {
                if(edges.Keys.Contains(OrderNodes(node, adjacentNode)))
                {
                    continue;
                }
                edges[OrderNodes(node, adjacentNode)] = Vector3.Distance(node.transform.position, adjacentNode.transform.position);
            }
        }
    }

    public void PrintGraph()
    {
        if(adjacencyMap == null)
        {
            Debug.Log("No Graph");
            return;
        }
        string line = "";
        foreach(var node in adjacencyMap.Keys)
        {
            line = "Node: " + node.gameObject.name;
            foreach(var adjacentNode in adjacencyMap[node])
            {
                line += "\t" + "Adjacent to: " + adjacentNode.gameObject.name + " distace: " + edges[OrderNodes(node, adjacentNode)];
            }
            Debug.Log(line);
        }
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        // If either are null
        if(startNode == null || targetNode == null)
        {
            Debug.Log("Tried to find a path beginning or ending at a null node");
            return new List<Node>();
        }
        bool targetAdjacentToStart = false;
        foreach(var node in adjacencyMap[startNode])
        {
            if(node.transform.position == targetNode.transform.position)
            {
                targetAdjacentToStart = true;
            }
        }
        if(targetAdjacentToStart)
        {
            List<Node> path = new List<Node>();
            path.Add(startNode);
            path.Add(targetNode);
            return path;
        }
        List<Node> openSet = new();
        HashSet<Node> closedSet = new();
        openSet.Add(startNode);

        Dictionary<Node, Node> cameFrom = new();
        Dictionary<Node, float> gScore = new();
        Dictionary<Node, float> fScore = new();

        foreach (var node in adjacencyMap.Keys)
        {
            gScore[node] = float.MaxValue;
            fScore[node] = float.MaxValue;
        }

        gScore[startNode] = 0f;
        fScore[startNode] = HeuristicCostEstimate(startNode, targetNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.OrderBy(node => fScore[node]).First();

            if (currentNode == targetNode)
            {
                return ReconstructPath(cameFrom, currentNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (var neighbor in adjacencyMap[currentNode])
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[currentNode] + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = currentNode;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + edges[OrderNodes(currentNode, neighbor)];
            }
        }

        return new List<Node>();
    }

    public List<Vector3> NodesListToVector3List(List<Node> nodes)
    {
        if (nodes == null)
            nodes = new List<Node>();
        return nodes.Select(node => node.transform.position).ToList();
    }

    private List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
    {
        List<Node> totalPath = new List<Node> { currentNode };

        while (cameFrom.ContainsKey(currentNode))
        {
            currentNode = cameFrom[currentNode];
            totalPath.Insert(0, currentNode);
        }

        return totalPath;
    }

    private float HeuristicCostEstimate(Node start, Node end)
    {
        if(start == null || end == null) 
            return float.MaxValue;
        return Vector3.Distance(start.transform.position, end.transform.position);
    }

    public (Node, Node) OrderNodes(Node alpha, Node beta)
    {
        Vector3 offsetOrigin = Vector3.left * 2000;
        float distanceFromOriginAlpha, distanceFromOriginBeta;
        distanceFromOriginAlpha = Vector3.Distance(alpha.transform.position, offsetOrigin);
        distanceFromOriginBeta = Vector3.Distance(beta.transform.position, offsetOrigin);
        if (distanceFromOriginAlpha < distanceFromOriginBeta)
        {
            return (alpha, beta);
        }
        else
        {
            return (beta, alpha);
        }
    }

    public Node FindAdjacentNodes(Vector3 position, string tag, float radius, LayerMask layerMask)
    {
        List<GameObject> newAdjacentNodes = new();

        // Circle cast to see all nearby nodes
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, radius, Vector2.right);
        // Debug.Log(hits.Length);
        foreach (var hit in hits)
        {
            // Check if the hit has the same tag and is not this gameObject
            if (hit.collider != null && hit.transform.gameObject.tag == tag && hit.transform.position != position)
            {
                // Check for line of sight
                Vector2 directionToTarget = hit.transform.position - position;
                RaycastHit2D[] sightTest = Physics2D.RaycastAll(position, directionToTarget, radius, layerMask);
                foreach (var hit2 in sightTest)
                {
                    if (hit2.transform.CompareTag("Walls"))
                    {
                        // Debug.Log("Hit a wall");
                        break;
                    }

                    if (hit2.collider != null && hit2.collider.gameObject == hit.transform.gameObject)
                    {
                        // Add to adjacent nodes if it shares a tag and is in line of sight
                        newAdjacentNodes.Add(hit.transform.gameObject);
                    }
                }
            }
        }
        if(newAdjacentNodes.Count > 0)
        {
            GameObject closestNode = newAdjacentNodes[0];
            float distance = Vector3.Distance(position, closestNode.transform.position);
            foreach(GameObject go in newAdjacentNodes)
            {
                float newDistance = Vector3.Distance(position, go.transform.position);
                if (newDistance < distance)
                {
                    // Debug.Log("Past closest: " + distance + " new closest: " + newDistance);
                    closestNode = go;
                    distance = newDistance;
                }
            }
            return closestNode.GetComponent<Node>();
        }
        return null;
    }
}
