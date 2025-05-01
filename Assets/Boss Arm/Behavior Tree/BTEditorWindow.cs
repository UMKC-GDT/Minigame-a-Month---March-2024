using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

#if UNITY_EDITOR
namespace BehaviorTree {
public class BTEditorWindow : EditorWindow {
    private BTNode draggedNode = null;
    private Vector2 dragOffset;

    private BTNode selectedParentNode = null;

    private BehaviorTreeAsset currentTree;
    private string savePath = "Assets/BehaviorTree.asset";

    private List<BTNode> nodes = new List<BTNode>();
    private Vector2 offset;
    private Vector2 drag;

    [MenuItem("Tools/Behavior Tree Editor")]
    static void OpenWindow() => GetWindow<BTEditorWindow>("Behavior Tree");

    void OnGUI() {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Tree")) SaveTree();
        if (GUILayout.Button("Load Tree")) LoadTree();
        GUILayout.EndHorizontal();

        //DrawGrid(20, 0.2f, Color.gray);
        //DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    void DrawNodes() {
        foreach (var node in nodes) {
            // Create custom style
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;

            // Background color (temporary)
            GUI.color = (node == selectedParentNode) ? Color.cyan : new Color(0.25f, 0.25f, 0.25f);

            GUI.Box(node.rect, node.title, style);
        }

        GUI.color = Color.white; // Reset after drawing
    }

    void ProcessEvents(Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    // Left click — try to start dragging a node
                    draggedNode = GetNodeAtPosition(e.mousePosition);
                    if (draggedNode != null)
                        dragOffset = e.mousePosition - draggedNode.rect.position;
                } else if (e.button == 1) {
                    ShowContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0)
                    draggedNode = null;
                break;

            case EventType.MouseDrag:
                if (e.button == 0) {
                    if (draggedNode != null) {
                        draggedNode.rect.position = e.mousePosition - dragOffset;
                        GUI.changed = true;
                    } else {
                        OnDrag(e.delta); // Pan view if not dragging a node
                    }
                }
                break;
        }
    }

    void OnDrag(Vector2 delta) {
        drag = delta;
        for (int i = 0; i < nodes.Count; i++) {
            nodes[i].rect.position += delta;
        }
        GUI.changed = true;
    }
    
    void DrawConnections() {
        foreach (var parent in nodes) {
            foreach (var child in parent.children) {
                Vector3 start = new Vector3(parent.rect.center.x, parent.rect.center.y);
                Vector3 end = new Vector3(child.rect.center.x, child.rect.center.y);
                Handles.DrawLine(start, end);
            }
        }
    }

    void ShowContextMenu(Vector2 mousePosition) {
        BTNode clickedNode = GetNodeAtPosition(mousePosition);
        GenericMenu menu = new GenericMenu();

        // Dynamically add all BTNode types (excluding abstract ones)
        var nodeTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(t => typeof(BehaviorTree.BTNode).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in nodeTypes) {
            string name = type.FullName.Replace("BehaviorTree.", "");
            menu.AddItem(new GUIContent($"Add/{name}"), false, () => AddNode(type, mousePosition));
        }

        if (clickedNode != null) {
            menu.AddItem(new GUIContent("Set as Parent"), false, () => selectedParentNode = clickedNode);
            if (selectedParentNode != null && selectedParentNode != clickedNode)
                menu.AddItem(new GUIContent("Link to Parent"), false, () => selectedParentNode.AddChild(clickedNode));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Delete Node"), false, () => DeleteNode(clickedNode));
        }

        menu.ShowAsContext();
    }


    BTNode GetNodeAtPosition(Vector2 pos) {
        return nodes.Find(n => n.rect.Contains(pos));
    }

    void AddNode(Vector2 position) {
        nodes.Add(new SequenceNode(position));
    }

    void AddNode(System.Type type, Vector2 position) {
        var node = (BehaviorTree.BTNode)System.Activator.CreateInstance(type, position);
        nodes.Add(node);
        GUI.changed = true;
    }

    void DeleteNode(BTNode node) {
        // Remove from all parent references
        foreach (var parent in nodes) {
            parent.children.Remove(node);
        }

        if (selectedParentNode == node)
            selectedParentNode = null;

        nodes.Remove(node);
        GUI.changed = true;
    }

    void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                             new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                             new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    void SaveTree() {
        var treeAsset = ScriptableObject.CreateInstance<BehaviorTreeAsset>();

        for (int i = 0; i < nodes.Count; i++) {
            var node = nodes[i];
            var data = new BTNodeData {
                typeName = node.GetType().Name,
                position = node.rect.position,
                childrenIndices = new List<int>()
            };

            foreach (var child in node.children) {
                int index = nodes.IndexOf(child);
                if (index >= 0) data.childrenIndices.Add(index);
            }

            treeAsset.nodes.Add(data);
        }

        AssetDatabase.CreateAsset(treeAsset, savePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Behavior tree saved.");
    }

    void LoadTree() {
        var treeAsset = AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(savePath);
        if (treeAsset == null) {
            Debug.LogWarning("Tree asset not found.");
            return;
        }

        nodes.Clear();

        foreach (var nodeData in treeAsset.nodes) {
            var pos = nodeData.position;
            Type nodeType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == nodeData.typeName && typeof(BTNode).IsAssignableFrom(t));

            if (nodeType != null) {
                BTNode node = (BTNode)Activator.CreateInstance(nodeType, pos);
                nodes.Add(node);
            } else {
                Debug.LogWarning($"Could not find node type: {nodeData.typeName}");
            }
        }

        for (int i = 0; i < treeAsset.nodes.Count; i++) {
            foreach (int childIndex in treeAsset.nodes[i].childrenIndices) {
                if (childIndex >= 0 && childIndex < nodes.Count) {
                    nodes[i].AddChild(nodes[childIndex]);
                }
            }
        }

        Debug.Log("Behavior tree loaded.");
    }
}
}
#endif
