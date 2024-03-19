using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Graph graph = (Graph)target;

        if (GUILayout.Button("Construct Graph"))
        {
            graph.ContstructGraph();
        }
        if (GUILayout.Button("Print Graph"))
        {
            graph.PrintGraph();
        }
        if(GUILayout.Button("Find Path"))
        {
            string line = "Path: ";
            List<Node> path = graph.FindPath(graph.start, graph.end);
            foreach (Node step in path)
            {
                line += " " + step.ToString();
            }

            Debug.Log(line);
        }
    }
}
